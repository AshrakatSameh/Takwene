using Takwene.Application.DTOs.Auth;

namespace Takwene.Application.Interfaces
{
    public interface IJwtTokenService
    {
        AuthResponse GenerateToken(string username);
    }
}
