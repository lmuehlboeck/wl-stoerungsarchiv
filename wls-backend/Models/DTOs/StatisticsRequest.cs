using wls_backend.Models.Enums;

namespace wls_backend.Models.DTOs;

public class StatisticsRequest
{
    public StatisticsValueAxis ValueAxis { get; set; }
    public TimeFrame TimeFrame { get; set; }
    public string Lines { get; set; } = "";
    public string Types { get; set; } = "";
    public DateOnly FromDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly ToDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}