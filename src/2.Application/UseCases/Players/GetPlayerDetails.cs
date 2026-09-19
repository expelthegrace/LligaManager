using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using LligaManager.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Application.UseCases.Players;

public sealed class GetPlayerDetails(
    IPlayerRepository playerRepository,
    ILogger<GetPlayerDetails>? logger = null)
{
    private readonly ILogger<GetPlayerDetails> logger = logger ?? NullLogger<GetPlayerDetails>.Instance;

    public async Task<Player> ExecuteAsync(
        PlayerId playerId,
        CancellationToken cancellationToken = default)
    {
        var player = await playerRepository.GetByIdAsync(playerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Player '{playerId}' was not found.");
        logger.LogInformation("Loaded player {PlayerId}.", player.Id);
        return player;
    }
}
