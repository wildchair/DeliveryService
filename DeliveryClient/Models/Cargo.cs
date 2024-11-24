using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryService.Models
{
    public class Cargo
    {
        [Key]
        public required int Id { get; set; }

        public required string Name { get; set; }

        public required float Weight { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required CargoSizeClass SizeClass { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Weight, SizeClass);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            return GetHashCode() == obj.GetHashCode();
        }
    }

    public enum CargoSizeClass
    {
        None,
        Small,
        Medium,
        Large
    }
}
