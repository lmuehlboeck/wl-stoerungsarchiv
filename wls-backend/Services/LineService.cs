using Microsoft.EntityFrameworkCore;
using wls_backend.Data;
using wls_backend.Models.DTOs;

namespace wls_backend.Services
{
    public class LineService
    {
        private readonly AppDbContext _context;
        public LineService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LineResponse>> GetLines()
        {
            var lines = (await _context.Line
                .ToListAsync())
                .OrderBy(l => l.Type)
                .ThenBy(l => {
                    try
                    {
                        return int.Parse(string.Concat(l.Id.Where(char.IsDigit)));
                    }
                    catch
                    {
                        return int.MinValue;
                    }
                })
                .ThenBy(l => l.Id)
                .ToList();
            return lines.Select(LineResponse.FromDomain);
        }
    }
}
