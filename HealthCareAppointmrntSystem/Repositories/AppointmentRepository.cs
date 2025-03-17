using HealthCareAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCareAppointmentSystem.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthCareContext _context;

        public AppointmentRepository(HealthCareContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentDetails>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        public async Task<AppointmentDetails> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentID == id);
        }

        public async Task AddAppointmentAsync(AppointmentDetails appointment)
        {
            // Ensure AppointmentID is not set explicitly
            appointment.AppointmentID = 0; // or default(int)
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(AppointmentDetails appointment)
        {
            var existingEntity = _context.Appointments.Local.FirstOrDefault(a => a.AppointmentID == appointment.AppointmentID);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task CancelAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Entry(appointment).State = EntityState.Detached;
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AppointmentDetails>> GetOverlappingAppointmentsAsync(DateTime date, TimeSpan timeSlot, int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.Date == date && a.TimeSlot == timeSlot && a.DoctorID == doctorId)
                .ToListAsync();
        }
    }
}