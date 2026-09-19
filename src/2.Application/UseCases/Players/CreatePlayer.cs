using LligaManager.Application.Abstractions;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LligaManager.Application.UseCases.Players;

public sealed record CreatePlayerRequest(
    string Name,
    PlayerRole Roles,
    PlayerStatus Status = PlayerStatus.Available,
    PlayerPriority Priority = PlayerPriority.Normal,
    string? Observations = null);

public sealed class CreatePlayer(
    IPlayerRepository playerRepository,
    ILogger<CreatePlayer>? logger = null)
{
    private readonly ILogger<CreatePlayer> logger = logger ?? NullLogger<CreatePlayer>.Instance;

    public async Task<Player> ExecuteAsync(
        CreatePlayerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var name = new PlayerName(request.Name);
        if (await playerRepository.ExistsByNameAsync(name, cancellationToken))
            throw new InvalidOperationException($"A player named '{name.Value}' already exists.");

        var player = Player.Create(
            name,
            request.Roles,
            request.Status,
            request.Priority,
            new Observations(request.Observations));

        await playerRepository.AddAsync(player, cancellationToken);
        logger.LogInformation("Player {PlayerId} created.", player.Id);
        return player;
    }
}
