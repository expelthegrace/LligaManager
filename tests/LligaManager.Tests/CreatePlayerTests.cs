using LligaManager.Application.Abstractions;
using LligaManager.Application.UseCases.Players;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;
using Xunit;

namespace LligaManager.Tests;

public sealed class CreatePlayerTests
{
    [Fact]
    public async Task ExecuteAsync_creates_and_persists_player_from_raw_input()
    {
        var repository = new InMemoryPlayerRepository();
        var useCase = new CreatePlayer(repository);

        var player = await useCase.ExecuteAsync(new CreatePlayerRequest(
            "  Alex  ",
            PlayerRole.Interior | PlayerRole.Exterior,
            Observations: "  Notes  "));

        Assert.Equal("Alex", player.Name.Value);
        Assert.Equal(PlayerRole.Interior | PlayerRole.Exterior, player.Roles);
        Assert.Equal("Notes", player.Observations.Value);
        Assert.Same(player, repository.AddedPlayer);
    }

    [Fact]
    public async Task ExecuteAsync_rejects_duplicate_names_before_persisting()
    {
        var repository = new InMemoryPlayerRepository();
        var existing = Player.Create(new PlayerName("Alex"), PlayerRole.Base);
        repository.Names.Add(existing.Name.Value);
        var useCase = new CreatePlayer(repository);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            useCase.ExecuteAsync(new CreatePlayerRequest(" Alex ", PlayerRole.Base)));

        Assert.Contains("already exists", exception.Message);
        Assert.Null(repository.AddedPlayer);
    }

    private sealed class InMemoryPlayerRepository : IPlayerRepository
    {
        public HashSet<string> Names { get; } = new(StringComparer.OrdinalIgnoreCase);
        public Player? AddedPlayer { get; private set; }

        public Task<bool> ExistsByNameAsync(
            PlayerName name,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(Names.Contains(name.Value));

        public Task<bool> ExistsByNameAsync(
            PlayerName name,
            PlayerId excludingId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(Names.Contains(name.Value));

        public Task<Player?> GetByIdAsync(
            PlayerId id,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<Player?>(null);

        public Task<IReadOnlyCollection<Player>> GetAllAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<Player>>([]);

        public Task<IReadOnlyCollection<Player>> GetAllAvailableAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<Player>>([]);

        public Task AddAsync(
            Player player,
            CancellationToken cancellationToken = default)
        {
            AddedPlayer = player;
            Names.Add(player.Name.Value);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(
            Player player,
            CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}
