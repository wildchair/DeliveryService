using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Repository
{
    public class DeliveryDb : DbContext
    {
        public DeliveryDb(DbContextOptions<DeliveryDb> options)
        : base(options) { }
        public DbSet<Cargo> Cargos => Set<Cargo>();
        public DbSet<Courier> Couriers => Set<Courier>();
        public DbSet<Order> Orders => Set<Order>();
    }
}
