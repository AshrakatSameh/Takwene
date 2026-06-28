using Takwene.Domain.Enums;

namespace Takwene.Domain.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;

        public string Isrc { get; set; } = string.Empty; // International Standard Recording Code
        public DateOnly ReleaseDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public TrackStatus Status { get; set; } = TrackStatus.Draft;

        public ICollection<TrackDistribution> Distributions { get; set; } = new List<TrackDistribution>();
    }
}
