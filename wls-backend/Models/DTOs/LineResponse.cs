using wls_backend.Models.Domain;
using wls_backend.Models.Enums;

namespace wls_backend.Models.DTOs
{
    public class LineResponse
    {
        public required string Id { get; set; }
        public required LineType Type { get; set; }
        public required string DisplayName { get; set; }

        public static LineResponse FromDomain(Line line)
        {
            return new LineResponse
            {
                Id = line.Id,
                Type = line.Type,
                DisplayName = line.DisplayName
            };
        }
    }
}
