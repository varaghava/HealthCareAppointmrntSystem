using HealthCareAppointmentSystem.Models;

namespace HealthCareAppointmentSystem.Authorization
{
    public interface IAuth
    {
        string GenerateToken(User user);
    }
}
