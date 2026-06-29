using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Takwene.Domain.Models;

namespace Takwene.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        // Ensures the demo admin user exists. The password is hashed with BCrypt
        // at runtime, so no plaintext password is ever stored in the database.
        public static async Task SeedAdminAsync(AppDbContext db, IConfiguration config)
        {
            var username = config["SeedAdmin:Username"] ?? "admin";
            var password = config["SeedAdmin:Password"] ?? "Admin123!";

            if (await db.Users.AnyAsync(u => u.Username == username))
                return;

            db.Users.Add(new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            });
            await db.SaveChangesAsync();
        }
    }
}
