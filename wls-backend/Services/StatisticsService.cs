using System.Globalization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using wls_backend.Data;
using wls_backend.Models.Domain;
using wls_backend.Models.DTOs;
using wls_backend.Models.Enums;

namespace wls_backend.Services;

public class StatisticsService
{
    private readonly AppDbContext _context;

    public StatisticsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StatisticsResponse> GetStatistics(StatisticsRequest request)
    {
        var query = _context.DisturbanceFiltered(new DisturbanceFilter
        {
            Lines = request.Lines,
            Types = request.Types,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
        }).Where(d => d.Type != DisturbanceType.ConstructionWork);
        Expression<Func<Disturbance, int>> timeSelector = request.TimeUnit switch
        {
            TimeUnit.Year => d => d.StartedAt < request.FromDate.ToDateTime(TimeOnly.MinValue)
                ? request.FromDate.Year
                : d.StartedAt.Year,
            TimeUnit.Month => d => d.StartedAt < request.FromDate.ToDateTime(TimeOnly.MinValue)
                ? request.FromDate.Month
                : d.StartedAt.Month,
            TimeUnit.Day => d => d.StartedAt < request.FromDate.ToDateTime(TimeOnly.MinValue)
                ? request.FromDate.Day
                : d.StartedAt.Day,
            TimeUnit.Hour => d => d.StartedAt < request.FromDate.ToDateTime(TimeOnly.MinValue)
                ? 0
                : d.StartedAt.Hour,
            _ =>  throw new NotImplementedException()
        };
        return request.ValueAxis switch
        {
            StatisticsValueAxis.NumberDisturbances => new StatisticsResponse
            {
                ByTime =
                    await query.GroupBy(timeSelector)
                        .Select(d => new DataPoint { X = d.Key.ToString(), Y = d.Count() })
                        .ToListAsync(),
                ByType = await query.GroupBy(d => d.Type)
                    .Select(d => new DataPoint { X = d.Key.ToString(), Y = d.Count() })
                    .ToListAsync(),
                ByLine = await query.SelectMany(d => d.Lines)
                    .GroupBy(l => l.Id)
                    .Select(l => new DataPoint { X = l.Key, Y = l.Count() })
                    .ToListAsync(),
            },
            StatisticsValueAxis.DurationDisturbed => new StatisticsResponse
            {
                ByTime = await query.GroupBy(timeSelector)
                    .Select(d => new DataPoint
                    {
                        X = d.Key.ToString(), Y = d.Sum(ds => (ds.EndedAt!.Value - ds.StartedAt).TotalHours)
                    })
                    .ToListAsync(),
                ByType = await query.GroupBy(d => d.Type)
                    .Select(d => new DataPoint
                    {
                        X = d.Key.ToString(), Y = d.Sum(ds => (ds.EndedAt!.Value - ds.StartedAt).TotalHours)
                    })
                    .ToListAsync(),
                ByLine = await query.SelectMany(d => d.Lines, (dist, line) => new { dist, line })
                    .GroupBy(e => e.line.Id)
                    .Select(e => new DataPoint
                    {
                        X = e.Key.ToString(), Y = e.Sum(es => (es.dist.EndedAt!.Value - es.dist.StartedAt).TotalHours)
                    })
                    .ToListAsync(),
            },
            _ => throw new NotImplementedException()
        };
    }
}