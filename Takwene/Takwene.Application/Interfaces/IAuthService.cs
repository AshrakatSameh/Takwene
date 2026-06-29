using System.Threading.Tasks;
using Takwene.Application.DTOs.Auth;

namespace Takwene.Application.Interfaces
{
    public interface IAuthService
    {
        // Returns a signed JWT on success, or null if credentials are invalid.
        Task<AuthResponse?> LoginAsync(LoginRequest request);
    }
}
