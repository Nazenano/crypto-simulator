using Microsoft.EntityFrameworkCore;
using CryptoSimulator.DataContext.Entities;

namespace CryptoSimulator.DataContext;

public class SQL : DbContext
{
    public SQL(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<PriceHistory> PriceHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Wallet>()
            .HasOne(w => w.User)
            .WithOne(u => u.Wallet)
            .HasForeignKey<Wallet>(w => w.UserId);

        modelBuilder.Entity<Portfolio>()
            .HasOne(p => p.User)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.CryptoCurrency)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CryptoId);
    }
}
