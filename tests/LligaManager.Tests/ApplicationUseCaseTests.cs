using LligaManager.Application.Abstractions;
using LligaManager.Application.UseCases.Convocatorias;
using LligaManager.Application.UseCases.Players;
using LligaManager.Domain.Entities;
using LligaManager.Domain.Enums;
using LligaManager.Domain.Services;
using LligaManager.Domain.ValueObjects;
using Xunit;

namespace LligaManager.Tests;

public sealed class ApplicationUseCaseTests
{
    [Fact]
    public async Task UpdatePlayer_updates_details_and_status_as_one_flow()
    {
        var player = Player.Create(new PlayerName("Alex"), PlayerRole.Base);
        var repository = new InMemoryPlayerRepository(player);

        var updated = await new UpdatePlayer(repository).ExecuteAsync(new UpdatePlayerRequest(
            player.Id,
            "Alex Updated",
            PlayerRole.Exterior,
            PlayerStatus.Unavailable,
            "Notes"));

        Assert.Equal("Alex Updated", updated.Name.Value);
        Assert.Equal(PlayerRole.Exterior, updated.Roles);
        Assert.Equal(PlayerStatus.Unavailable, updated.Status);
        Assert.Equal("Notes", updated.Observations.Value);
        Assert.Same(player, repository.UpdatedPlayer);
    }

    [Fact]
    public async Task CreateConvocatoria_rejects_duplicate_dates()
    {
        var repository = new InMemoryConvocatoriaRepository();
        var useCase = new CreateConvocatoria(repository);
        var request = new CreateConvocatoriaRequest(new DateOnly(2026, 9, 19));

        await useCase.ExecuteAsync(request);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            useCase.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteConvocatoriaRaffle_loads_candidates_and_persists_result()
    {
        var winner = Player.Create(new PlayerName("Winner"), PlayerRole.Base);
        var loser = Player.Create(new PlayerName("Loser"), PlayerRole.Base);
        var convocatoria = Convocatoria.Create(new ConvocatoriaDate(new DateOnly(2026, 9, 19)));
        var players = new InMemoryPlayerRepository(winner, loser);
        var convocatorias = new InMemoryConvocatoriaRepository(convocatoria);

        var result = await new ExecuteConvocatoriaRaffle(
            convocatorias,
            players,
            new FixedConfigurationProvider(1)).ExecuteAsync(convocatoria.Id);

        Assert.Single(result.RaffleWinners);
        Assert.Single(result.RaffleLosers);
        Assert.Equal(1, convocatorias.SaveChangesCount);
    }

    private sealed class FixedConfigurationProvider(int maximumPlayers)
        : IConvocatoriaConfigurationProvider
    {
        public ConvocatoriaConfiguration Load() => new(maximumPlayers);
    }

    private sealed class InMemoryPlayerRepository(params Player[] initialPlayers) : IPlayerRepository
    {
        private readonly List<Player> players = [.. initialPlayers];
        public Player? UpdatedPlayer { get; private set; }

        public Task<bool> ExistsByNameAsync(
            PlayerName name,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(players.Any(player => player.Name == name));

        public Task<bool> ExistsByNameAsync(
            PlayerName name,
            PlayerId excludingId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(players.Any(player => player.Id != excludingId && player.Name == name));

        public Task<Player?> GetByIdAsync(
            PlayerId id,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(players.SingleOrDefault(player => player.Id == id));

        public Task<IReadOnlyCollection<Player>> GetAllAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<Player>>(players);

        public Task<IReadOnlyCollection<Player>> GetAllAvailableAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<Player>>(
                players.Where(player => player.Status == PlayerStatus.Available).ToList());

        public Task AddAsync(Player player, CancellationToken cancellationToken = default)
        {
            players.Add(player);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Player player, CancellationToken cancellationToken = default)
        {
            UpdatedPlayer = player;
            return Task.CompletedTask;
        }
    }

    private sealed class InMemoryConvocatoriaRepository(
        params Convocatoria[] initialConvocatorias) : IConvocatoriaRepository
    {
        private readonly List<Convocatoria> convocatorias = [.. initialConvocatorias];
        public int SaveChangesCount { get; private set; }

        public Task<bool> ExistsByDateAsync(
            ConvocatoriaDate date,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(convocatorias.Any(convocatoria => convocatoria.Date == date));

        public Task<Convocatoria?> GetByIdAsync(
            ConvocatoriaId id,
            bool includeRaffleDetails = false,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(convocatorias.SingleOrDefault(convocatoria => convocatoria.Id == id));

        public Task<IReadOnlyCollection<Convocatoria>> GetAllAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<Convocatoria>>(convocatorias);

        public Task AddAsync(
            Convocatoria convocatoria,
            CancellationToken cancellationToken = default)
        {
            convocatorias.Add(convocatoria);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(
            Convocatoria convocatoria,
            CancellationToken cancellationToken = default) =>
            Task.CompletedTask;

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveChangesCount++;
            return Task.CompletedTask;
        }
    }
}
