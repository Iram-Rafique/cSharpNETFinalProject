// using Microsoft.EntityFrameworkCore;
// using albumsEntities;

// namespace albumsContext
// {
//     // this must be db name
//     public class chinookDb : DbContext
//     {

       
//         // one customer go to customers table public class Customer is used in your DbContext

//         public DbSet<Album> Albums { get; set; }
//         public DbSet<Artist> Artists { get; set; }
//         public DbSet<Track> Tracks { get; set; }

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

//         // must cahge file name it is database name
//         {
//             optionsBuilder.UseSqlite("Data Source=chinook.db");
//         }
//     }
// }
using Microsoft.EntityFrameworkCore;
using albumsEntities;

namespace albumsContext
{
    // database context for the Chinook database
    public class chinookDb : DbContext
    {
        // parameterless constructor allows simple usage: new chinookDb()
        public chinookDb() { }

        // DbSets representing tables in the database
        public DbSet<Album> Albums { get; set; } = null!;
        public DbSet<Artist> Artists { get; set; } = null!;
        public DbSet<Track> Tracks { get; set; } = null!;

        // Configure SQLite database connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=chinook.db");
            }
        }
    }
}