using Microsoft.EntityFrameworkCore;
using wls_backend.Data;
using wls_backend.Models.DTOs;

namespace wls_backend.Services
{
    public class DisturbanceService
    {
        private readonly AppDbContext _context;

        public DisturbanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DisturbanceResponse?> GetDisturbance(string id)
        {
            var disturbance = await _context.DisturbanceWithAll.FirstOrDefaultAsync(d => d.Id == id);
            if (disturbance == null)
            {
                return null;
            }

            disturbance.Lines = disturbance.Lines
                .OrderBy(l => l.Type)
                .ThenBy(_context.LineOrderSelector)
                .ThenBy(l => l.Id)
                .ToList();
            return DisturbanceResponse.FromDomain(disturbance);
        }

        public async Task<IEnumerable<DisturbanceResponse>> GetDisturbances(DisturbanceFilter filter)
        {
            var disturbances = await _context.DisturbanceFiltered(filter).ToListAsync();
            return disturbances.Select(DisturbanceResponse.FromDomain);
        }
    }
}