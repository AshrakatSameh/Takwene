using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Takwene.Application.DTOs.Artists;
using Takwene.Application.Interfaces;
using Takwene.Domain.Models;
using Takwene.Infrastructure.Persistence;

namespace Takwene.Infrastructure.Services
{
    public class ArtistService : IArtistService
    {
        private readonly AppDbContext _db;
        private readonly IValidator<CreateArtistRequest> _validator;

        public ArtistService(AppDbContext db, IValidator<CreateArtistRequest> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task<ArtistDto> CreateAsync(CreateArtistRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);

            var artist = new Artist
            {
                Name = request.Name,
                Email = request.Email,
                Country = request.Country
            };

            _db.Artists.Add(artist);
            await _db.SaveChangesAsync();

            return new ArtistDto(artist.Id, artist.Name, artist.Email, artist.Country);
        }

        public async Task<IReadOnlyList<ArtistDto>> GetAllAsync()
        {
            return await _db.Artists
                .OrderBy(a => a.Name)
                .Select(a => new ArtistDto(a.Id, a.Name, a.Email, a.Country))
                .ToListAsync();
        }
    }
}
