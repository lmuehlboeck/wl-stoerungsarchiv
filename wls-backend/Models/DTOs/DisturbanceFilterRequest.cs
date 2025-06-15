using wls_backend.Models.Enums;

namespace wls_backend.Models.DTOs
{
    public class DisturbanceFilterRequest
    {
        public string Lines { get; set; } = "";
        public string Types { get; set; } = "";
        public bool OnlyActive { get; set; } = false;
        public DateOnly FromDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly ToDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public OrderType OrderBy { get; set; } = OrderType.StartedAtDesc;
    }
}
