namespace wls_backend.Models.Domain
{
    public class DisturbanceDescription
    {
        required public string DisturbanceId { get; set; }

        required public string Text { get; set; }
        required public DateTime CreatedAt { get; set; }
    }
}
