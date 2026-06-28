using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Takwene.Application.DTOs.Tracks;
using Takwene.Application.Exceptions;
using Takwene.Application.Interfaces;
using Takwene.Domain.Enums;
using Takwene.Domain.Models;
using Takwene.Infrastructure.Persistence;

namespace Takwene.Infrastructure.Services
{
    public class TrackService : ITrackService
    {
        private readonly AppDbContext _db;
        private readonly IValidator<CreateTrackRequest> _createValidator;
        private readonly IValidator<DistributeRequest> _distributeValidator;
        private readonly IValidator<UpdateTrackStatusRequest> _statusValidator;

        public TrackService(
            AppDbContext db,
            IValidator<CreateTrackRequest> createValidator,
            IValidator<DistributeRequest> distributeValidator,
            IValidator<UpdateTrackStatusRequest> statusValidator)
        {
            _db = db;
            _createValidator = createValidator;
            _distributeValidator = distributeValidator;
            _statusValidator = statusValidator;
        }

        public async Task<TrackDetailDto> CreateAsync(CreateTrackRequest request)
        {
            await _createValidator.ValidateAndThrowAsync(request);

            var artistExists = await _db.Artists.AnyAsync(a => a.Id == request.ArtistId);
            if (!artistExists)
                throw new NotFoundException($"Artist {request.ArtistId} not found.");

            var isrcTaken = await _db.Tracks.AnyAsync(t => t.Isrc == request.Isrc);
            if (isrcTaken)
                throw new ConflictException($"A track with ISRC '{request.Isrc}' already exists.");

            var track = new Track
            {
                Title = request.Title,
                ArtistId = request.ArtistId,
                Isrc = request.Isrc,
                ReleaseDate = request.ReleaseDate,
                Genre = request.Genre,
                Status = TrackStatus.Draft
            };

            _db.Tracks.Add(track);
            await _db.SaveChangesAsync();

            return (await GetByIdAsync(track.Id))!;
        }

        public async Task<IReadOnlyList<TrackListItemDto>> GetListAsync(int? artistId, string? genre, TrackStatus? status)
        {
            var query = _db.Tracks.AsNoTracking().Include(t => t.Artist).AsQueryable();

            if (artistId.HasValue)
                query = query.Where(t => t.ArtistId == artistId.Value);

            if (!string.IsNullOrWhiteSpace(genre))
                query = query.Where(t => t.Genre == genre);

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            return await query
                .OrderBy(t => t.Title)
                .Select(t => new TrackListItemDto(
                    t.Id,
                    t.Title,
                    t.Artist.Name,
                    t.Genre,
                    t.Status.ToString()))
                .ToListAsync();
        }

        public async Task<TrackDetailDto?> GetByIdAsync(int id)
        {
            return await _db.Tracks
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TrackDetailDto(
                    t.Id,
                    t.Title,
                    t.ArtistId,
                    t.Artist.Name,
                    t.Isrc,
                    t.ReleaseDate,
                    t.Genre,
                    t.Status.ToString(),
                    t.Distributions
                        .OrderBy(d => d.Dsp.Name)
                        .Select(d => new DistributionDto(
                            d.DspId,
                            d.Dsp.Name,
                            d.Status.ToString(),
                            d.SubmittedAt))
                        .ToList()))
                .FirstOrDefaultAsync();
        }

        public async Task<TrackDetailDto> DistributeAsync(int trackId, DistributeRequest request)
        {
            await _distributeValidator.ValidateAndThrowAsync(request);

            var track = await _db.Tracks
                .Include(t => t.Distributions)
                .FirstOrDefaultAsync(t => t.Id == trackId)
                ?? throw new NotFoundException($"Track {trackId} not found.");

            var validDspIds = await _db.Dsps.Select(d => d.Id).ToListAsync();
            var unknown = request.DspIds.Distinct().Except(validDspIds).ToList();
            if (unknown.Count > 0)
                throw new NotFoundException($"Unknown DSP id(s): {string.Join(", ", unknown)}.");

            var alreadyTargeted = track.Distributions.Select(d => d.DspId).ToHashSet();
            foreach (var dspId in request.DspIds.Distinct().Where(id => !alreadyTargeted.Contains(id)))
            {
                _db.TrackDistributions.Add(new TrackDistribution
                {
                    TrackId = trackId,
                    DspId = dspId,
                    SubmittedAt = DateTime.UtcNow,
                    Status = DistributionStatus.Pending
                });
            }

            // Submitting a draft track to a DSP moves it into the Submitted state.
            if (track.Status == TrackStatus.Draft)
                track.Status = TrackStatus.Submitted;

            await _db.SaveChangesAsync();

            return (await GetByIdAsync(trackId))!;
        }

        public async Task<TrackDetailDto> UpdateStatusAsync(int trackId, UpdateTrackStatusRequest request)
        {
            await _statusValidator.ValidateAndThrowAsync(request);

            var track = await _db.Tracks.FirstOrDefaultAsync(t => t.Id == trackId)
                ?? throw new NotFoundException($"Track {trackId} not found.");

            track.Status = request.Status;
            await _db.SaveChangesAsync();

            return (await GetByIdAsync(trackId))!;
        }
    }
}
