using wls_backend.Models.Domain;
using wls_backend.Models.Enums;

namespace wls_backend.Models.DTOs
{
    public class DisturbanceResponse
    {
        required public string Id { get; set; }
        required public string Title { get; set; }
        required public DisturbanceType Type { get; set; }
        required public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; } = null;
        required public ICollection<DisturbanceDescriptionResponse> Descriptions { get; set; } = [];
        required public ICollection<LineResponse> Lines { get; set; } = [];

        public static DisturbanceResponse FromDomain(Disturbance disturbance)
        {
            return new DisturbanceResponse
            {
                Id = disturbance.Id,
                Title = disturbance.Title,
                Type = disturbance.Type,
                StartedAt = disturbance.StartedAt,
                EndedAt = disturbance.EndedAt,
                Descriptions = disturbance.Descriptions.Select(d => new DisturbanceDescriptionResponse
                {
                    Text = d.Text,
                    CreatedAt = d.CreatedAt
                }).ToList(),
                Lines = disturbance.Lines.Select(LineResponse.FromDomain).ToList()
            };
        }
    }
}
