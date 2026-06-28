using System.Collections.Generic;
using System.Threading.Tasks;
using Takwene.Application.DTOs.Artists;

namespace Takwene.Application.Interfaces
{
    public interface IArtistService
    {
        Task<ArtistDto> CreateAsync(CreateArtistRequest request);
        Task<IReadOnlyList<ArtistDto>> GetAllAsync();
    }
}
