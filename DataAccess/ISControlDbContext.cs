using System.Data.Entity;
using DataAccess.DbModels;

namespace DataAccess
{
    public class ISControlDbContext : DbContext
    {
        public ISControlDbContext() : base("ISControl")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inspection>().HasRequired(x => x.Customer).WithMany(x => x.Inspections).WillCascadeOnDelete(false);
        }

        public DbSet<Company> Company { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Event> Event { get; set; }

        public DbSet<Inspection> Inspection { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}