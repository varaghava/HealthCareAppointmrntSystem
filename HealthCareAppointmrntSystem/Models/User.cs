using System.ComponentModel.DataAnnotations;

namespace HealthCareAppointmentSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; } // "Doctor" or "Patient"

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Address { get; set; }
    }
}