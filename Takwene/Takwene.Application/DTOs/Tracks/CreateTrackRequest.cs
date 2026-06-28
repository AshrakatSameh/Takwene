using System;

namespace Takwene.Application.DTOs.Tracks
{
    public record CreateTrackRequest(
        string Title,
        int ArtistId,
        string Isrc,
        DateOnly ReleaseDate,
        string Genre);
}
