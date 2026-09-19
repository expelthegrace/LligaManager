using Xunit;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.Services;
using LligaManager.Domain.ValueObjects;

namespace LligaManager.Tests;

public sealed class RaffleTests
{
    [Fact]
    public void Execute_selects_available_high_priority_first_and_updates_results()
    {
        var high = Player.Create(
            new PlayerName("High"),
            PlayerRole.Base,
            priority: PlayerPriority.High);
        var normal = Player.Create(new PlayerName("Normal"), PlayerRole.Base);
        var unavailable = Player.Create(
            new PlayerName("Unavailable"),
            PlayerRole.Base,
            status: PlayerStatus.Unavailable);
        var convocatoria = CreateConvocatoria();

        ExecuteRaffle([normal, high, unavailable], convocatoria, maximumPlayers: 1);

        Assert.Equal([high], convocatoria.RaffleWinners);
        Assert.Equal([normal], convocatoria.RaffleLosers);
        Assert.Equal([high], convocatoria.CurrentPlayers);
        Assert.Equal(PlayerPriority.Normal, high.Priority);
        Assert.Equal(PlayerPriority.High, normal.Priority);
        Assert.Equal(PlayerPriority.Normal, unavailable.Priority);
    }

    [Fact]
    public void Execute_marks_available_non_winners_as_high_priority_and_ignores_duplicates()
    {
        var first = Player.Create(new PlayerName("First"), PlayerRole.Base);
        var second = Player.Create(new PlayerName("Second"), PlayerRole.Base);
        var convocatoria = CreateConvocatoria();

        ExecuteRaffle([first, first, second], convocatoria, maximumPlayers: 1);

        Assert.Single(convocatoria.RaffleWinners);
        Assert.Single(convocatoria.RaffleLosers);
        Assert.Equal(PlayerPriority.High, convocatoria.RaffleLosers.Single().Priority);
    }

    [Fact]
    public void Execute_rejects_reassigning_raffle_results()
    {
        var player = Player.Create(new PlayerName("Player"), PlayerRole.Base);
        var convocatoria = CreateConvocatoria();
        var raffle = new Raffle(new Random(1));

        raffle.Execute([player], convocatoria, new ConvocatoriaConfiguration(1));

        Assert.Throws<InvalidOperationException>(() =>
            raffle.Execute([player], convocatoria, new ConvocatoriaConfiguration(1)));
    }

    [Fact]
    public void Configuration_requires_a_positive_maximum()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ConvocatoriaConfiguration(0));
    }

    private static Convocatoria CreateConvocatoria() =>
        Convocatoria.Create(new ConvocatoriaDate(new DateOnly(2026, 9, 19)));

    private static void ExecuteRaffle(
        IReadOnlyCollection<Player> players,
        Convocatoria convocatoria,
        int maximumPlayers) =>
        new Raffle(new Random(1)).Execute(
            players,
            convocatoria,
            new ConvocatoriaConfiguration(maximumPlayers));
}
