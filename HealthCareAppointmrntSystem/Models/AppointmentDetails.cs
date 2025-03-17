using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCareAppointmentSystem.Models
{
    public class AppointmentDetails
    {
        [Key]
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Appointment date must be today or a future date.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "TimeSlot is required.")]
        public TimeSpan TimeSlot { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        public string Status { get; set; } = "Booked"; // Default status is "Booked"

        public User Patient { get; set; }
        public User Doctor { get; set; }
    }

    // Custom validation attribute for future dates
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date.Date < DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}