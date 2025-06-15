using wls_backend.Models.Enums;

namespace wls_backend.Models.Domain
{
    public class Line
    {
        required public string Id { get; set; }
        required public LineType Type { get; set; }
        required public string DisplayName { get; set; }

        public ICollection<Disturbance> Disturbances { get; set; } = [];
    }
}
