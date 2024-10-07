using Microsoft.EntityFrameworkCore;

namespace EFCorePagination;

public class MoviesDbContext : DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Movies.db");
    }
}
