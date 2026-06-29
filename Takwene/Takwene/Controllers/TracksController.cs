using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Takwene.Application.DTOs.Tracks;
using Takwene.Application.Interfaces;
using Takwene.Domain.Enums;

namespace Takwene.Api.Controllers
{
    [ApiController]
    [Route("api/tracks")]
    public class TracksController : ControllerBase
    {
        private readonly ITrackService _service;

        public TracksController(ITrackService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TrackDetailDto>> Create(CreateTrackRequest request)
        {
            var track = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = track.Id }, track);
        }

        // GET /api/tracks?artistId=&genre=&status=
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TrackListItemDto>>> GetList(
            [FromQuery] int? artistId,
            [FromQuery] string? genre,
            [FromQuery] TrackStatus? status)
        {
            return Ok(await _service.GetListAsync(artistId, genre, status));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrackDetailDto>> GetById(int id)
        {
            var track = await _service.GetByIdAsync(id);
            return track is null ? NotFound() : Ok(track);
        }

        [HttpPost("{id:int}/distribute")]
        [Authorize]
        public async Task<ActionResult<TrackDetailDto>> Distribute(int id, DistributeRequest request)
        {
            return Ok(await _service.DistributeAsync(id, request));
        }

        [HttpPatch("{id:int}/status")]
        [Authorize]
        public async Task<ActionResult<TrackDetailDto>> UpdateStatus(int id, UpdateTrackStatusRequest request)
        {
            return Ok(await _service.UpdateStatusAsync(id, request));
        }
    }
}
