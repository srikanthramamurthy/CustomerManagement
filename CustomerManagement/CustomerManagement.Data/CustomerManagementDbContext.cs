using System.Reflection;
using CustomerManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CustomerManagement.Data
{
    public class CustomerManagementDbContext : DbContext
    {
        public CustomerManagementDbContext()
        {
        }

        public CustomerManagementDbContext(DbContextOptions<CustomerManagementDbContext> options,
            IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}