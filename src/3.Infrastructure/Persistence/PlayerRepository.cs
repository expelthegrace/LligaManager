using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using LligaManager.Application.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Infrastructure.Persistence;

public sealed class PlayerRepository(
    LligaManagerDbContext dbContext,
    ILogger<PlayerRepository>? logger = null) : IPlayerRepository
{
    private readonly ILogger<PlayerRepository> logger = logger ?? NullLogger<PlayerRepository>.Instance;

    public Task<bool> ExistsByNameAsync(
        PlayerName name,
        CancellationToken cancellationToken = default) =>
        dbContext.Players.AnyAsync(player => player.Name == name, cancellationToken);

    public Task<bool> ExistsByNameAsync(
        PlayerName name,
        PlayerId excludingId,
        CancellationToken cancellationToken = default) =>
        dbContext.Players.AnyAsync(
            player => player.Name == name && player.Id != excludingId,
            cancellationToken);

    public async Task<Player?> GetByIdAsync(
        PlayerId id,
        CancellationToken cancellationToken = default) =>
        await dbContext.Players.SingleOrDefaultAsync(player => player.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Player>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        await dbContext.Players.OrderBy(player => player.Name.Value).ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Player>> GetAllAvailableAsync(
        CancellationToken cancellationToken = default) =>
        await dbContext.Players
            .Where(player => player.Status == PlayerStatus.Available)
            .OrderBy(player => player.Name.Value)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(
        Player player,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Players.AddAsync(player, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Player {PlayerId} persisted.", player.Id);
    }

    public async Task UpdateAsync(
        Player player,
        CancellationToken cancellationToken = default)
    {
        dbContext.Players.Update(player);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Player {PlayerId} updated.", player.Id);
    }
}
