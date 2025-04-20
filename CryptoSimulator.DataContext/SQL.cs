using Microsoft.EntityFrameworkCore;

namespace CryptoSimulator.DataContext;

public class SQL : DbContext
{
    public SQL(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
