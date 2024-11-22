namespace DeliveryService.Models
{
    public class Courier
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Phone { get; set; }
        public bool IsCarCourier { get; set; }
    }
}
