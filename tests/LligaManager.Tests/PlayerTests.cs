using Xunit;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;

namespace LligaManager.Tests;

public sealed class PlayerTests
{
    [Fact]
    public void Create_sets_defaults_and_preserves_multiple_roles()
    {
        var player = Player.Create(
            new PlayerName("  Alex  "),
            PlayerRole.Interior | PlayerRole.Exterior);

        Assert.NotEqual(default, player.Id);
        Assert.Equal("Alex", player.Name.Value);
        Assert.Equal(PlayerRole.Interior | PlayerRole.Exterior, player.Roles);
        Assert.Equal(PlayerStatus.Available, player.Status);
        Assert.Equal(PlayerPriority.Normal, player.Priority);
        Assert.Equal(string.Empty, player.Observations.Value);
    }

    [Theory]
    [InlineData(PlayerRole.None)]
    public void Create_rejects_players_without_a_role(PlayerRole roles)
    {
        Assert.Throws<ArgumentException>(() =>
            Player.Create(new PlayerName("Alex"), roles));
    }

    [Fact]
    public void UpdateDetails_updates_editable_data_but_rejects_empty_roles()
    {
        var player = Player.Create(new PlayerName("Alex"), PlayerRole.Base);

        player.UpdateDetails(
            new PlayerName("  Alex Updated "),
            PlayerRole.Exterior,
            new Observations("  Updated notes "));

        Assert.Equal("Alex Updated", player.Name.Value);
        Assert.Equal(PlayerRole.Exterior, player.Roles);
        Assert.Equal("Updated notes", player.Observations.Value);
        Assert.Throws<ArgumentException>(() =>
            player.UpdateDetails(new PlayerName("Alex"), PlayerRole.None, new Observations()));
    }
}
