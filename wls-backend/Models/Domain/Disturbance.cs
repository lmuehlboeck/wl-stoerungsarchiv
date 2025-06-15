using wls_backend.Models.Enums;

namespace wls_backend.Models.Domain
{
    public class Disturbance
    {
        required public string Id { get; set; }
        required public string Title { get; set; }
        required public DisturbanceType Type { get; set; }
        required public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; } = null;

        required public ICollection<DisturbanceDescription> Descriptions { get; set; }
        required public ICollection<Line> Lines { get; set; }
    }
}
