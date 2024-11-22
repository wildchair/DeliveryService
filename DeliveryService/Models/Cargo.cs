namespace DeliveryService.Models
{
    public class Cargo
    {
        public required string Name { get; set; }
        public required float Weight { get; set; }
        public required CargoSizeClass SizeClass { get; set; }
    }

    public enum CargoSizeClass
    {
        None,
        Small,
        Medium,
        Large
    }
}
