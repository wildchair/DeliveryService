namespace DeliveryService.Models
{
    public class NewOrderDto
    {
        public required int Id { get; set; }

        public required string Address { get; set; }

        public required DateTime DeliveryTime { get; set; }

        public int? CourierId { get; set; }

        public required Cargo Cargo { get; set; }
    }
}
