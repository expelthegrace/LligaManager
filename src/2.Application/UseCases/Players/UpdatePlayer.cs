using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Application.UseCases.Players;

public sealed record UpdatePlayerRequest(
    PlayerId PlayerId,
    string Name,
    PlayerRole Roles,
    PlayerStatus Status,
    string? Observations = null);

public sealed class UpdatePlayer(
    IPlayerRepository playerRepository,
    ILogger<UpdatePlayer>? logger = null)
{
    private readonly ILogger<UpdatePlayer> logger = logger ?? NullLogger<UpdatePlayer>.Instance;

    public async Task<Player> ExecuteAsync(
        UpdatePlayerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var player = await playerRepository.GetByIdAsync(request.PlayerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Player '{request.PlayerId}' was not found.");
        var name = new PlayerName(request.Name);

        if (await playerRepository.ExistsByNameAsync(name, player.Id, cancellationToken))
            throw new InvalidOperationException($"A player named '{name.Value}' already exists.");

        player.UpdateDetails(name, request.Roles, new Observations(request.Observations));
        player.SetStatus(request.Status);
        await playerRepository.UpdateAsync(player, cancellationToken);
        logger.LogInformation("Player {PlayerId} updated.", player.Id);
        return player;
    }
}
