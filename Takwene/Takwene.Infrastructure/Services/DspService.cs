using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Takwene.Application.DTOs.Dsps;
using Takwene.Application.Interfaces;
using Takwene.Infrastructure.Persistence;

namespace Takwene.Infrastructure.Services
{
    public class DspService : IDspService
    {
        private readonly AppDbContext _db;

        public DspService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<DspDto>> GetAllAsync()
        {
            return await _db.Dsps
                .OrderBy(d => d.Name)
                .Select(d => new DspDto(d.Id, d.Name))
                .ToListAsync();
        }
    }
}
