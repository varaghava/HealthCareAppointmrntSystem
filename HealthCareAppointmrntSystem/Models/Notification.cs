using System;
using System.ComponentModel.DataAnnotations;
using HealthCareAppointmentSystem.Models;

namespace HealthCareAppointmentSystem.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime SentDate { get; set; }

        public bool IsRead { get; set; }

        public User User { get; set; }
    }
}