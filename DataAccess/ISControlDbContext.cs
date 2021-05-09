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
            modelBuilder.Entity<Inspection>()
                .HasRequired(x => x.Customer)
                .WithMany(x => x.Inspections)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(x => x.Employees)
                .WithOptional(x => x.Company)
                .WillCascadeOnDelete(true);
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Inspection> Inspections { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Document> Documents { get; set; }
    }
}