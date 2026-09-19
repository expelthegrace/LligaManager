using LligaManager.Domain.Entities;
using LligaManager.Domain.ValueObjects;

namespace LligaManager.Application.Abstractions;

public interface IPlayerRepository
{
    Task<bool> ExistsByNameAsync(PlayerName name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(PlayerName name, PlayerId excludingId, CancellationToken cancellationToken = default);
    Task<Player?> GetByIdAsync(PlayerId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Player>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Player>> GetAllAvailableAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Player player, CancellationToken cancellationToken = default);
    Task UpdateAsync(Player player, CancellationToken cancellationToken = default);
}
