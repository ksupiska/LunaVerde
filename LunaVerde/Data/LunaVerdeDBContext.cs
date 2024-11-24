using Microsoft.EntityFrameworkCore;
using LunaVerde.Models;

namespace LunaVerde.Data
{
    public class LunaVerdeDBContext : DbContext
    {
        public LunaVerdeDBContext(DbContextOptions<LunaVerdeDBContext> options) : base(options) { }

        public DbSet<Menu> Menu { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
