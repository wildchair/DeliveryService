using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
    }

    public enum CargoSizeClass
    {
        None,
        Small,
        Medium,
        Large
    }
}
