using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAcessLayer.DBContext
{
    public class EcDbContext : DbContext
    {
        public EcDbContext(DbContextOptions<EcDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrdersTable> OrderTable { get; set; }
    }
}