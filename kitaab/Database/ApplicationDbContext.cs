using kitaab.Model;
using Microsoft.EntityFrameworkCore;

namespace kitaab.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ) : base (options) {}
    
    public DbSet<Book> Books { get; set; }
}