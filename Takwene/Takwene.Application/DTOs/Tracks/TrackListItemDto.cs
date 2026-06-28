namespace Takwene.Application.DTOs.Tracks
{
    public record TrackListItemDto(
        int Id,
        string Title,
        string ArtistName,
        string Genre,
        string Status);
}
