using Microsoft.EntityFrameworkCore;

namespace DotNetBack.Data
{
    public class FifteenPuzzleDbContext : DbContext
    {
        public DbSet<UserGameResult> UserGameResults { get; set; }

        public FifteenPuzzleDbContext(DbContextOptions<FifteenPuzzleDbContext> options)
            : base(options)
        {
        }
    }
}
