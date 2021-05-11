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
                .HasForeignKey(x => x.CustomerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(x => x.Employees)
                .WithOptional(x => x.Company)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Inspection>()
                .HasRequired(x => x.Contractor)
                .WithMany(x => x.OrderedInspections)
                .HasForeignKey(x => x.ContractorId)
                .WillCascadeOnDelete(false);
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Inspection> Inspections { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}