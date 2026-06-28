using System;
using Microsoft.EntityFrameworkCore;
using Takwene.Domain.Enums;
using Takwene.Domain.Models;

namespace Takwene.Infrastructure.Persistence
{
    public static class SeedData
    {
        // Fixed UTC timestamp for all seeded distributions (deterministic for HasData).
        private static readonly DateTime SeedTime =
            new DateTime(2025, 01, 15, 10, 0, 0, DateTimeKind.Utc);

        public static void Apply(ModelBuilder b)
        {
            // ---- DSPs (3) ----
            b.Entity<Dsp>().HasData(
                new Dsp { Id = 1, Name = "Spotify" },
                new Dsp { Id = 2, Name = "Apple Music" },
                new Dsp { Id = 3, Name = "YouTube" });

            // ---- Artists (3, varied countries) ----
            b.Entity<Artist>().HasData(
                new Artist { Id = 1, Name = "Layla Hassan",  Email = "layla@example.com",  Country = "Saudi Arabia" },
                new Artist { Id = 2, Name = "Marcus Lee",    Email = "marcus@example.com", Country = "United States" },
                new Artist { Id = 3, Name = "Nadia Petrova", Email = "nadia@example.com",  Country = "United Kingdom" });

            // ---- Tracks (9, across genres and all three statuses) ----
            b.Entity<Track>().HasData(
                new Track { Id = 1, Title = "Desert Dawn",  ArtistId = 1, Isrc = "SAA112500001", ReleaseDate = new DateOnly(2025, 2, 1),  Genre = "Pop",        Status = TrackStatus.Distributed },
                new Track { Id = 2, Title = "Night Drive",  ArtistId = 2, Isrc = "USRC17600002", ReleaseDate = new DateOnly(2024, 11, 20), Genre = "Hip-Hop",    Status = TrackStatus.Submitted },
                new Track { Id = 3, Title = "Echoes",       ArtistId = 3, Isrc = "GBA112500003", ReleaseDate = new DateOnly(2025, 1, 10), Genre = "Electronic", Status = TrackStatus.Distributed },
                new Track { Id = 4, Title = "Golden Hour",  ArtistId = 1, Isrc = "SAA112500004", ReleaseDate = new DateOnly(2025, 3, 5),  Genre = "R&B",        Status = TrackStatus.Draft },
                new Track { Id = 5, Title = "City Lights",  ArtistId = 2, Isrc = "USRC17600005", ReleaseDate = new DateOnly(2024, 9, 15), Genre = "Rock",       Status = TrackStatus.Distributed },
                new Track { Id = 6, Title = "Mirage",       ArtistId = 1, Isrc = "SAA112500006", ReleaseDate = new DateOnly(2025, 2, 18), Genre = "Electronic", Status = TrackStatus.Submitted },
                new Track { Id = 7, Title = "Reckless",     ArtistId = 2, Isrc = "USRC17600007", ReleaseDate = new DateOnly(2025, 4, 1),  Genre = "Pop",        Status = TrackStatus.Draft },
                new Track { Id = 8, Title = "Aurora",       ArtistId = 3, Isrc = "GBA112500008", ReleaseDate = new DateOnly(2024, 12, 25), Genre = "Indie",      Status = TrackStatus.Distributed },
                new Track { Id = 9, Title = "Sandstorm",    ArtistId = 1, Isrc = "SAA112500009", ReleaseDate = new DateOnly(2025, 3, 22), Genre = "Afrobeat",   Status = TrackStatus.Submitted });

            // ---- Track distributions (mixed statuses) ----
            b.Entity<TrackDistribution>().HasData(
                // Track 1 (Distributed) -> live on all three
                new TrackDistribution { Id = 1,  TrackId = 1, DspId = 1, SubmittedAt = SeedTime, Status = DistributionStatus.Live },
                new TrackDistribution { Id = 2,  TrackId = 1, DspId = 2, SubmittedAt = SeedTime, Status = DistributionStatus.Live },
                new TrackDistribution { Id = 3,  TrackId = 1, DspId = 3, SubmittedAt = SeedTime, Status = DistributionStatus.Live },
                // Track 3 (Distributed)
                new TrackDistribution { Id = 4,  TrackId = 3, DspId = 1, SubmittedAt = SeedTime, Status = DistributionStatus.Live },
                new TrackDistribution { Id = 5,  TrackId = 3, DspId = 3, SubmittedAt = SeedTime, Status = DistributionStatus.Live },
                // Track 5 (Distributed) - one rejected
                new TrackDistribution { Id = 6,  TrackId = 5, DspId = 1, SubmittedAt = SeedTime, Status = DistributionStatus.Live },
                new TrackDistribution { Id = 7,  TrackId = 5, DspId = 2, SubmittedAt = SeedTime, Status = DistributionStatus.Rejected },
                // Track 8 (Distributed)
                new TrackDistribution { Id = 8,  TrackId = 8, DspId = 2, SubmittedAt = SeedTime, Status = DistributionStatus.Live },
                // Track 2 (Submitted) - pending
                new TrackDistribution { Id = 9,  TrackId = 2, DspId = 1, SubmittedAt = SeedTime, Status = DistributionStatus.Pending },
                // Track 6 (Submitted) - pending
                new TrackDistribution { Id = 10, TrackId = 6, DspId = 3, SubmittedAt = SeedTime, Status = DistributionStatus.Pending },
                // Track 9 (Submitted) - pending on two
                new TrackDistribution { Id = 11, TrackId = 9, DspId = 1, SubmittedAt = SeedTime, Status = DistributionStatus.Pending },
                new TrackDistribution { Id = 12, TrackId = 9, DspId = 2, SubmittedAt = SeedTime, Status = DistributionStatus.Pending });
        }
    }
}
