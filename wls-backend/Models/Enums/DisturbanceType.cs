namespace wls_backend.Models.Enums
{
    public enum DisturbanceType
    {
        Misc,
        Delay,
        Accident,
        AmbulanceOperation,
        FireDepartmentOperation,
        PoliceOperation,
        ParkingOffender,
        DefectiveVehicle,
        CatenaryDamage,
        TrackDamage,
        SignalDamage,
        SwitchDamage,
        ConstructionWork,
        Demonstration,
        Event,
        Weather,
    }

    public class DisturbanceTypeHelper
    {
        public static DisturbanceType FromTitle(string title)
        {
            var sc = StringComparison.OrdinalIgnoreCase;
            return title switch
            {
                string t when t.Contains("Verspätung", sc) => DisturbanceType.Delay,
                string t when t.Contains("Unfall", sc) => DisturbanceType.Accident,
                string t when t.Contains("Rettung", sc) => DisturbanceType.AmbulanceOperation,
                string t when t.Contains("Feuerwehr", sc) => DisturbanceType.FireDepartmentOperation,
                string t when t.Contains("Polizei", sc) => DisturbanceType.PoliceOperation,
                string t when t.Contains("Falschparker", sc) => DisturbanceType.ParkingOffender,
                string t when t.Contains("Fahrzeug", sc) => DisturbanceType.DefectiveVehicle,
                string t when t.Contains("Fahrleitung", sc) || t.Contains("Oberleitung", sc) => DisturbanceType.CatenaryDamage,
                string t when t.Contains("Gleisschaden", sc) => DisturbanceType.TrackDamage,
                string t when t.Contains("Signal", sc) => DisturbanceType.SignalDamage,
                string t when t.Contains("Weiche", sc) => DisturbanceType.SwitchDamage,
                string t when t.Contains("Bauarbeiten", sc) => DisturbanceType.ConstructionWork,
                string t when t.Contains("Demonstration", sc) => DisturbanceType.Demonstration,
                string t when t.Contains("Veranstaltung", sc) => DisturbanceType.Event,
                string t when t.Contains("Witterung", sc) => DisturbanceType.Weather,
                _ => DisturbanceType.Misc,
            };
        }
    }
}
