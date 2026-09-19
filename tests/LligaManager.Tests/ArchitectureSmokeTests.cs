using Xunit;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.Services;
using LligaManager.Domain.ValueObjects;

namespace LligaManager.Tests;

public sealed class ArchitectureSmokeTests
{
    [Fact]
    public void Test_project_is_configured()
    {
        Assert.True(true);
    }

    [Fact]
    public void Raffle_prefers_high_priority_players_and_updates_priorities()
    {
        var high = Player.Create(new PlayerName("High"), PlayerRole.Base, priority: PlayerPriority.High);
        var normal = Player.Create(new PlayerName("Normal"), PlayerRole.Base);
        var convocatoria = Convocatoria.Create(new ConvocatoriaDate(new DateOnly(2026, 9, 19)));

        new Raffle(new Random(1)).Execute(
            [normal, high],
            convocatoria,
            new ConvocatoriaConfiguration(1));

        Assert.Single(convocatoria.RaffleWinners);
        Assert.Contains(high, convocatoria.RaffleWinners);
        Assert.Equal(PlayerPriority.Normal, high.Priority);
        Assert.Equal(PlayerPriority.High, normal.Priority);
    }

    [Fact]
    public void Convocatoria_does_not_allow_raffle_results_to_be_reassigned()
    {
        var player = Player.Create(new PlayerName("Player"), PlayerRole.Base);
        var convocatoria = Convocatoria.Create(new ConvocatoriaDate(new DateOnly(2026, 9, 19)));
        var raffle = new Raffle(new Random(1));
        raffle.Execute([player], convocatoria, new ConvocatoriaConfiguration(1));

        Assert.Throws<InvalidOperationException>(() =>
            raffle.Execute([player], convocatoria, new ConvocatoriaConfiguration(1)));
    }
}
