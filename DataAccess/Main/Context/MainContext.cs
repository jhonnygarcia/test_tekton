using Application.Main;
using Domain.Main.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Main.Context
{
    public class MainContext : DbContext, IMainContext
    {
        public MainContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {       
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly, type =>
            {
                return type.Namespace.StartsWith("DataAccess.Main.Configurations");
            });
        }
    }
}
