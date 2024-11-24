using DeliveryService.Models;
using System.Text.Json.Serialization;

namespace DeliveryClient.Dto
{
    public class UpdateOrderDto
    {
        public string Address { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public int CourierId { get; set; }

        public Cargo Cargo { get; set; }

        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderState? State { get; set; }
    }
}
