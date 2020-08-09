using Diploma.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.DBConnection
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Positions> Positions{ get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<PersonalData> PersonalData { get; set; }

            public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=staff;Username=postgres;Password=08febr1998");
        }
    }
}
