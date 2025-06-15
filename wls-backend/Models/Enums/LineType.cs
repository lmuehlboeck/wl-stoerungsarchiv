namespace wls_backend.Models.Enums
{
    public enum LineType
    {
        Misc,
        Metro,
        Tram,
        Bus,
        Night
    }

    public static class LineTypeHelper
    {
        public static LineType FromType(string type)
        {
            return type switch
            {
                "ptMetro" => LineType.Metro,
                "ptTram" or "ptTramWLB" => LineType.Tram,
                "ptBusCity" => LineType.Bus,
                "ptBusNight" => LineType.Night,
                _ => LineType.Misc
            };
        }
    }
}
