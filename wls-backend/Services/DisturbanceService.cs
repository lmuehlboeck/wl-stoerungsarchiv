using Microsoft.EntityFrameworkCore;
using wls_backend.Data;
using wls_backend.Models.Domain;
using wls_backend.Models.DTOs;
using wls_backend.Models.Enums;

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
            return DisturbanceResponse.FromDomain(disturbance);
        }

        public async Task<IEnumerable<DisturbanceResponse>> GetDisturbances(DisturbanceFilterRequest filter)
        {
            var fromDate = filter.FromDate.ToDateTime(TimeOnly.MinValue);
            var toDate = filter.ToDate.ToDateTime(TimeOnly.MaxValue);
            var query = _context.DisturbanceWithAll
                .Where(d => (d.StartedAt >= fromDate && d.StartedAt <= toDate)
                            || ((d.EndedAt ?? DateTime.Now) >= fromDate && (d.EndedAt ?? DateTime.Now) <= toDate));
            
            if(!string.IsNullOrWhiteSpace(filter.Lines))
            {
                var lines = filter.Lines.Split(',').Select(l => l.Trim()).ToList();
                query = query.Where(d => d.Lines.Any(l => lines.Contains(l.Id)));
            }
            if (!string.IsNullOrWhiteSpace(filter.Types))
            {
                var types = filter.Types.Split(',').Select(t => Enum.Parse<DisturbanceType>(t.Trim())).ToList();
                query = query.Where(d => types.Contains(d.Type));
            }
            if (filter.OnlyActive)
            {
                query = query.Where(d => d.EndedAt == null);
            }
            query = filter.OrderBy switch
            {
                OrderType.StartedAtAsc => query.OrderBy(d => d.StartedAt),
                OrderType.StartedAtDesc => query.OrderByDescending(d => d.StartedAt),
                OrderType.EndedAtAsc => query.OrderBy(d => d.EndedAt),
                OrderType.EndedAtDesc => query.OrderByDescending(d => d.EndedAt),
                _ => throw new NotImplementedException(),
            };
            var disturbances = await query.ToListAsync();
            return disturbances.Select(DisturbanceResponse.FromDomain);
        }
    }
}
