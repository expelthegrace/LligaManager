using LligaManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LligaManager.Infrastructure.Persistence;

public sealed class LligaManagerDbContext(DbContextOptions<LligaManagerDbContext> options)
    : DbContext(options)
{
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Convocatoria> Convocatorias => Set<Convocatoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LligaManagerDbContext).Assembly);
    }
}
