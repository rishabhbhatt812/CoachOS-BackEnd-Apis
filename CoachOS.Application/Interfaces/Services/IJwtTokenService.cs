using CoachOS.Domain.Identity;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user, string roleCode);
    }
}
