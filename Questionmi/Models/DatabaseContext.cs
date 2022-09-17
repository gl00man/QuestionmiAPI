using Microsoft.EntityFrameworkCore;

namespace Questionmi.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<Tell> Tells { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<BadWord> BadWords { get; set; }
    }
}
