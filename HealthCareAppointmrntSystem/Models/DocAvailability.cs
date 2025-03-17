using System;
using System.ComponentModel.DataAnnotations;
using HealthCareAppointmentSystem.Models;

namespace HealthCareAppointmentSystem.Models
{
    public class DocAvailability
    {
        [Key]
        public int SessionID { get; set; }

        [Required]
        public int DoctorID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan TimeSlot { get; set; }

        public User Doctor { get; set; }
    }
}