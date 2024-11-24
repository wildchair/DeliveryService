using DeliveryService.Models;

namespace DeliveryService.Extensions
{
    public static class CargoExtensions
    {
        public static void CopyFrom(this Cargo cargo, Cargo updateCargo)
        {
            cargo.Id = updateCargo.Id;
            cargo.Name = updateCargo.Name;
            cargo.Weight = updateCargo.Weight;
            cargo.SizeClass = updateCargo.SizeClass;
        }
    }
}