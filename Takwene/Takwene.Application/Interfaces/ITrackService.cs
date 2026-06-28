using System.Collections.Generic;
using System.Threading.Tasks;
using Takwene.Application.DTOs.Tracks;
using Takwene.Domain.Enums;

namespace Takwene.Application.Interfaces
{
    public interface ITrackService
    {
        Task<TrackDetailDto> CreateAsync(CreateTrackRequest request);
        Task<IReadOnlyList<TrackListItemDto>> GetListAsync(int? artistId, string? genre, TrackStatus? status);
        Task<TrackDetailDto?> GetByIdAsync(int id);
        Task<TrackDetailDto> DistributeAsync(int trackId, DistributeRequest request);
        Task<TrackDetailDto> UpdateStatusAsync(int trackId, UpdateTrackStatusRequest request);
    }
}
