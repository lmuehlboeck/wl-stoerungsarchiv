using wls_backend.Models.Domain;
using wls_backend.Models.Enums;

namespace wls_backend.Models.DTOs
{
    public class LineResponse
    {
        required public string Id { get; set; }
        required public LineType Type { get; set; }
        required public string DisplayName { get; set; }

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
