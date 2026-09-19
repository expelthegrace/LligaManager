using Xunit;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;

namespace LligaManager.Tests;

public sealed class ConvocatoriaTests
{
    [Fact]
    public void Create_starts_pending_and_allows_details_and_status_changes()
    {
        var convocatoria = Convocatoria.Create(
            new ConvocatoriaDate(new DateOnly(2026, 9, 19)),
            new Observations("Initial"));

        Assert.Equal(ConvocatoriaStatus.Pending, convocatoria.Status);
        Assert.Equal("Initial", convocatoria.Observations.Value);
        Assert.Empty(convocatoria.CurrentPlayers);
        Assert.Empty(convocatoria.RaffleWinners);
        Assert.Empty(convocatoria.RaffleLosers);

        convocatoria.UpdateDetails(
            new ConvocatoriaDate(new DateOnly(2026, 9, 20)),
            new Observations("Updated"));
        convocatoria.SetStatus(ConvocatoriaStatus.Played);

        Assert.Equal(new DateOnly(2026, 9, 20), convocatoria.Date.Value);
        Assert.Equal("Updated", convocatoria.Observations.Value);
        Assert.Equal(ConvocatoriaStatus.Played, convocatoria.Status);
    }
}
