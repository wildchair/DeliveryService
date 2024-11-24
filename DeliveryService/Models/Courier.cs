using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Models
{
    public class Courier
    {
        [Key]
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Phone { get; set; }
        public bool IsCarCourier { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Surname, Phone, IsCarCourier);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            return GetHashCode() == obj.GetHashCode();
        }
    }
}
