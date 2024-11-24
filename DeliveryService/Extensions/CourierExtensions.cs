using DeliveryService.Models;

namespace DeliveryService.Extensions
{
    public static class CourierExtensions
    {
        public static Courier CopyFrom(this Courier courier, Courier updateCourier)
        {
            courier.Id = updateCourier.Id;
            courier.Name = updateCourier.Name;
            courier.Surname = updateCourier.Surname;
            courier.Phone = updateCourier.Phone;
            courier.IsCarCourier = updateCourier.IsCarCourier;
            return courier;
        }
    }
}
