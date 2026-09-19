using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Application.UseCases.Players;

public sealed class GetPlayers(
    IPlayerRepository playerRepository,
    ILogger<GetPlayers>? logger = null)
{
    private readonly ILogger<GetPlayers> logger = logger ?? NullLogger<GetPlayers>.Instance;

    public async Task<IReadOnlyCollection<Player>> ExecuteAsync(
        bool availableOnly = false,
        CancellationToken cancellationToken = default)
    {
        var players = availableOnly
            ? await playerRepository.GetAllAvailableAsync(cancellationToken)
            : await playerRepository.GetAllAsync(cancellationToken);
        logger.LogInformation("Loaded {PlayerCount} players.", players.Count);
        return players;
    }
}
