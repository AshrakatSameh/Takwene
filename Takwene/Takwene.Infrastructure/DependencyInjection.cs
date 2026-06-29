using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takwene.Application.Interfaces;
using Takwene.Application.Validators;
using Takwene.Infrastructure.Authentication;
using Takwene.Infrastructure.Persistence;
using Takwene.Infrastructure.Services;

namespace Takwene.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            services.Configure<JwtSettings>(config.GetSection("Jwt"));

            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<ITrackService, TrackService>();
            services.AddScoped<IDspService, DspService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();

            // Registers every AbstractValidator<T> in the Application assembly.
            services.AddValidatorsFromAssemblyContaining<CreateArtistRequestValidator>();

            return services;
        }
    }
}
