using HealthCareAppointmentSystem.Models;
using HealthCareAppointmentSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCareAppointmentSystem.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly NotificationService _notificationService;

        public AppointmentService(IAppointmentRepository appointmentRepository, NotificationService notificationService)
        {
            _appointmentRepository = appointmentRepository;
            _notificationService = notificationService;
        }

        public async Task<List<AppointmentDetails>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAppointmentsAsync();
        }

        public async Task<AppointmentDetails> GetAppointmentByIdAsync(int id)
        {
            return await _appointmentRepository.GetAppointmentByIdAsync(id);
        }

        public async Task AddAppointmentAsync(AppointmentDetails appointment)
        {
            if (appointment.Date.Date < DateTime.Today)
            {
                throw new ArgumentException("Appointment date must be today or a future date.");
            }

            var overlappingAppointments = await _appointmentRepository.GetOverlappingAppointmentsAsync(appointment.Date, appointment.TimeSlot, appointment.DoctorID);
            if (overlappingAppointments.Any())
            {
                throw new ArgumentException("An appointment already exists for the same date and time.");
            }

            appointment.Status = "Booked";
            await _appointmentRepository.AddAppointmentAsync(appointment);
        }

        public async Task UpdateAppointmentAsync(AppointmentDetails appointment)
        {
            appointment.Status = "Updated";
            await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }

        public async Task CancelAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                await _appointmentRepository.UpdateAppointmentAsync(appointment);

                var message = $"Your appointment with Dr. {appointment.Doctor.Name} on {appointment.Date} at {appointment.TimeSlot} has been cancelled.";
                await _notificationService.SendNotificationAsync(appointment.PatientID, message);
            }
        }

        public async Task RescheduleAppointmentAsync(int id, DateTime newDate, TimeSpan newTimeSlot)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
            if (appointment != null)
            {
                if (newDate.Date < DateTime.Today)
                {
                    throw new ArgumentException("Appointment date must be today or a future date.");
                }

                var overlappingAppointments = await _appointmentRepository.GetOverlappingAppointmentsAsync(newDate, newTimeSlot, appointment.DoctorID);
                if (overlappingAppointments.Any())
                {
                    throw new ArgumentException("An appointment already exists for the same date and time.");
                }

                appointment.Date = newDate;
                appointment.TimeSlot = newTimeSlot;
                appointment.Status = "Rescheduled";
                await _appointmentRepository.UpdateAppointmentAsync(appointment);

                var message = $"Your appointment with Dr. {appointment.Doctor.Name} has been rescheduled to {newDate} at {newTimeSlot}.";
                await _notificationService.SendNotificationAsync(appointment.PatientID, message);
            }
        }
    }
}
