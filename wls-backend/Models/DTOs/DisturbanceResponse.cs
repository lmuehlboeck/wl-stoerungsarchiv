using wls_backend.Models.Domain;
using wls_backend.Models.Enums;

namespace wls_backend.Models.DTOs
{
    public class DisturbanceResponse
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required DisturbanceType Type { get; set; }
        public required DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; } = null;
        public required ICollection<DisturbanceDescriptionResponse> Descriptions { get; set; } = [];
        public required ICollection<LineResponse> Lines { get; set; } = [];

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
