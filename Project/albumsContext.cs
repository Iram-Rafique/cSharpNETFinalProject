using Microsoft.EntityFrameworkCore;
using albumsEntities;

namespace albumsContext
{
    // this must be db name
    public class chinookDb : DbContext
    {
        // one customer go to customers table public class Customer is used in your DbContext

        public DbSet<Album> Albums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        // must cahge file name it is database name
        {
            optionsBuilder.UseSqlite("Filename=chinook.db");
        }
    }
}