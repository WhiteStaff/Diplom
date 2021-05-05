using System.Data.Entity;

namespace DataAccess
{
    public class ISControlDbContext : DbContext
    {
        public ISControlDbContext() : base("ISControl")
        {
        }
    }
}