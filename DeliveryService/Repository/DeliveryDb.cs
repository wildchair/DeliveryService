using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Repository
{
    public class DeliveryDb(DbContextOptions<DeliveryDb> options) : DbContext(options)
    {
        public DbSet<Cargo> Cargos => Set<Cargo>();
        public DbSet<Courier> Couriers => Set<Courier>();
        public DbSet<Order> Orders => Set<Order>();
    }
}