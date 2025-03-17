using Microsoft.AspNetCore.Mvc;
using HealthCareAppointmentSystem.Models;
using HealthCareAppointmentSystem.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCareAppointmentSystem.Controllers
{
    [Route("api/[controller]")] // Route for the controller (e.g., /api/Appointment)
    [ApiController] // Indicates this is an API controller
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService; // Service dependency

        // Constructor: Dependency injection of AppointmentService
        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService; // Initialize the service
        }

        // GET: api/Appointment
        [HttpGet] // Handles HTTP GET requests
        public async Task<ActionResult<List<AppointmentDetails>>> GetAllAppointments()
        {
            // Call the service to get all appointments
            return await _appointmentService.GetAllAppointmentsAsync();
        }

        // GET: api/Appointment/5
        [HttpGet("{id}")] // Handles HTTP GET requests with an ID parameter
        public async Task<ActionResult<AppointmentDetails>> GetAppointmentById(int id)
        {
            // Call the service to get an appointment by ID
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            // If the appointment is not found, return 404 Not Found
            if (appointment == null)
            {
                return NotFound();
            }

            // Return the appointment
            return appointment;
        }

        // POST: api/Appointment
        [HttpPost] // Handles HTTP POST requests
        public async Task<ActionResult> AddAppointment([FromBody] AppointmentDetails appointment)
        {
            // Validate the model state (e.g., required fields)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 Bad Request if validation fails
            }

            try
            {
                // Call the service to add a new appointment
                await _appointmentService.AddAppointmentAsync(appointment);

                // Return 201 Created with the new appointment's location
                return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.AppointmentID }, appointment);
            }
            catch (ArgumentException ex)
            {
                // Handle validation errors from the service layer
                return BadRequest(ex.Message); // Return 400 Bad Request with the error message
            }
        }

        // PUT: api/Appointment/5
        [HttpPut("{id}")] // Handles HTTP PUT requests with an ID parameter
        public async Task<ActionResult> UpdateAppointment(int id, [FromBody] AppointmentDetails appointment)
        {
            // Check if the ID in the URL matches the ID in the request body
            if (id != appointment.AppointmentID)
            {
                return BadRequest(); // Return 400 Bad Request if IDs don't match
            }

            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 Bad Request if validation fails
            }

            try
            {
                // Call the service to update the appointment
                await _appointmentService.UpdateAppointmentAsync(appointment);

                // Return 204 No Content (successful update)
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                // Handle validation errors from the service layer
                return BadRequest(ex.Message); // Return 400 Bad Request with the error message
            }
        }

        // DELETE: api/Appointment/5
        [HttpDelete("{id}")] // Handles HTTP DELETE requests with an ID parameter
        public async Task<ActionResult> CancelAppointment(int id)
        {
            try
            {
                // Call the service to cancel the appointment
                await _appointmentService.CancelAppointmentAsync(id);

                // Return 204 No Content (successful deletion)
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                // Handle validation errors from the service layer
                return BadRequest(ex.Message); // Return 400 Bad Request with the error message
            }
        }

        // POST: api/Appointment/5/reschedule
        [HttpPost("{id}/reschedule")] // Handles HTTP POST requests for rescheduling
        public async Task<ActionResult> RescheduleAppointment(int id, [FromBody] RescheduleRequest request)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 Bad Request if validation fails
            }

            try
            {
                // Call the service to reschedule the appointment
                await _appointmentService.RescheduleAppointmentAsync(id, request.NewDate, request.NewTimeSlot);

                // Return 204 No Content (successful rescheduling)
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                // Handle validation errors from the service layer
                return BadRequest(ex.Message); // Return 400 Bad Request with the error message
            }
        }
    }

    // Class to represent the reschedule request body
    public class RescheduleRequest
    {
        public DateTime NewDate { get; set; } // New date for the appointment
        public TimeSpan NewTimeSlot { get; set; } // New time slot for the appointment
    }
}