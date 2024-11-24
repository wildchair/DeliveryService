using DeliveryService.Models;

namespace DeliveryService.Extensions
{
    public static class OrderExtensions
    {
        public static void FromUpdateOrderDto(this Order order, UpdateOrderDto updateOrderDto)
        {
            order.Address = updateOrderDto.Address ?? order.Address;
            order.DeliveryTime = updateOrderDto.DeliveryTime ?? order.DeliveryTime;
            order.Description = updateOrderDto.Description ?? order.Description;
            order.State = updateOrderDto.State ?? order.State;
            if (updateOrderDto.Courier != null)
                order.Courier.CopyFrom(updateOrderDto.Courier);
            if (updateOrderDto.Cargo != null)
                order.Cargo.CopyFrom(updateOrderDto.Cargo);
        }
    }
}
