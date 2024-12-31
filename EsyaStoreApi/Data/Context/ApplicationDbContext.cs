using EsyaStore.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace EsyaStore.Data.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext (DbContextOptions options):base(options)
        {

        }

        public DbSet<Users> users { get; set; }
        public DbSet<Sellers> sellers { get; set; }

        public DbSet<Products> products { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<Cart> cart { get; set; }   
        public DbSet<Reviews> reviews { get; set; }   

    }
}
