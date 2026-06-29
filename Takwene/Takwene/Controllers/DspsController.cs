using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Takwene.Application.DTOs.Dsps;
using Takwene.Application.Interfaces;

namespace Takwene.Api.Controllers
{
    [ApiController]
    [Route("api/dsps")]
    [Authorize]
    public class DspsController : ControllerBase
    {
        private readonly IDspService _service;

        public DspsController(IDspService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DspDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}
