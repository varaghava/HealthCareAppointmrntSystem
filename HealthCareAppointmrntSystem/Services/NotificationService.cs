using HealthCareAppointmentSystem.Models;
using HealthCareAppointmentSystem.Repositories;
using System.Threading.Tasks;

namespace HealthCareAppointmentSystem.Services
{
    public class NotificationService
    {
        private readonly HealthCareContext _context;

        public NotificationService(HealthCareContext context)
        {
            _context = context;
        }

        public async Task SendNotificationAsync(int userId, string message)
        {
            var notification = new Notification
            {
                UserID = userId,
                Message = message,
                SentDate = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}