using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;

namespace LligaManager.Domain.Services;

public sealed record ConvocatoriaConfiguration
{
    public ConvocatoriaConfiguration(int maximumPlayers)
    {
        if (maximumPlayers <= 0)
            throw new ArgumentOutOfRangeException(nameof(maximumPlayers));

        MaximumPlayers = maximumPlayers;
    }

    public int MaximumPlayers { get; }
}

public sealed class Raffle
{
    private readonly Random random;

    public Raffle(Random? random = null)
    {
        this.random = random ?? Random.Shared;
    }

    public void Execute(
        IReadOnlyCollection<Player> candidates,
        Convocatoria convocatoria,
        ConvocatoriaConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(candidates);
        ArgumentNullException.ThrowIfNull(convocatoria);
        ArgumentNullException.ThrowIfNull(configuration);

        var available = candidates
            .Where(player => player.Status == PlayerStatus.Available)
            .DistinctBy(player => player.Id)
            .OrderBy(_ => random.Next())
            .ToList();

        var highPriority = available
            .Where(player => player.Priority == PlayerPriority.High)
            .ToList();
        var normalPriority = available
            .Where(player => player.Priority == PlayerPriority.Normal)
            .ToList();

        var selected = highPriority
            .Concat(normalPriority)
            .Take(configuration.MaximumPlayers)
            .ToList();
        var selectedIds = selected.Select(player => player.Id).ToHashSet();
        var losers = available.Where(player => !selectedIds.Contains(player.Id)).ToList();

        foreach (var winner in selected)
            winner.SetPriority(PlayerPriority.Normal);
        foreach (var loser in losers)
            loser.SetPriority(PlayerPriority.High);

        convocatoria.AssignRaffle(selected, losers);
    }
}
