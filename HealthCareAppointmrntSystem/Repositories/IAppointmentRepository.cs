using HealthCareAppointmentSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCareAppointmentSystem.Repositories
{
    public interface IAppointmentRepository
    {
        Task<List<AppointmentDetails>> GetAllAppointmentsAsync();
        Task<AppointmentDetails> GetAppointmentByIdAsync(int id);
        Task AddAppointmentAsync(AppointmentDetails appointment);
        Task UpdateAppointmentAsync(AppointmentDetails appointment);
        Task CancelAppointmentAsync(int id);
        Task<List<AppointmentDetails>> GetOverlappingAppointmentsAsync(DateTime date, TimeSpan timeSlot, int doctorId);
    }
}