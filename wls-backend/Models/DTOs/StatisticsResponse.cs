namespace wls_backend.Models.DTOs;

public class StatisticsResponse
{
    public required ICollection<DataPoint> ByTime { get; set; }
    public required ICollection<DataPoint> ByType { get; set; }
    public required ICollection<DataPoint> ByLine { get; set; }
}

public class DataPoint
{
    public required string X { get; set; }
    public double Y { get; set; }
}
