using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Takwene.Application.DTOs.Auth;
using Takwene.Application.Interfaces;
using Takwene.Infrastructure.Persistence;

namespace Takwene.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IJwtTokenService _jwt;

        public AuthService(AppDbContext db, IJwtTokenService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            return _jwt.GenerateToken(user.Username);
        }
    }
}
