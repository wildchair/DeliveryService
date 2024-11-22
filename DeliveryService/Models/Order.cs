using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryService.Models
{
    public class Order
    {
        [Key]
        public required int Id { get; set; }

        public required string Address { get; set; }

        public required DateTime DeliveryTime { get; set; }

        public Courier? Courier { get; set; }

        public string? Description { get; set; }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required OrderState State { get; set; }
    }

    public enum OrderState
    {
        New,
        InProgress,
        Completed,
        Cancelled
    }
}
