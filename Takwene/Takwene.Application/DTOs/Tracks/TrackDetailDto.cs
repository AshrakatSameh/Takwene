using System;
using System.Collections.Generic;

namespace Takwene.Application.DTOs.Tracks
{
    public record TrackDetailDto(
        int Id,
        string Title,
        int ArtistId,
        string ArtistName,
        string Isrc,
        DateOnly ReleaseDate,
        string Genre,
        string Status,
        IReadOnlyList<DistributionDto> Distributions);
}
