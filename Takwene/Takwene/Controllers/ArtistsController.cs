using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Takwene.Application.DTOs.Artists;
using Takwene.Application.Interfaces;

namespace Takwene.Api.Controllers
{
    [ApiController]
    [Route("api/artists")]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _service;

        public ArtistsController(IArtistService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ArtistDto>> Create(CreateArtistRequest request)
        {
            var artist = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetAll), new { id = artist.Id }, artist);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ArtistDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}
