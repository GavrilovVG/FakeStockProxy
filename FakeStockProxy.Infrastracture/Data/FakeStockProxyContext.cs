using FakeStockProxy.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FakeStockProxy.Infrastructure.Data;

public class FakeStockProxyContext : DbContext
{
    public DbSet<FsStock> FsStocks => Set<FsStock>();
    public DbSet<FsStockItem> FsStockItems => Set<FsStockItem>();
    public DbSet<FsStockRequest> FsStockRequests => Set<FsStockRequest>();

    public FakeStockProxyContext(DbContextOptions<FakeStockProxyContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FsStockRequest>()
            .HasIndex(fr => fr.Hashsum)
            .IsUnique();

        modelBuilder.Entity<FsStockRequest>()
            .HasOne(sr => sr.FsStock)
            .WithOne(s => s.FsStockRequest)
            .HasForeignKey<FsStock>(s => s.FsStockRequestId);

        modelBuilder.Entity<FsStockItem>()
            .HasOne(si => si.FsStock)
            .WithMany(s => s.FsStockItems)
            .HasForeignKey(si => si.FsStockId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
