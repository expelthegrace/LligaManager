using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;

namespace LligaManager.Domain.Entities;

public sealed class Convocatoria
{
    private readonly List<Player> currentPlayers = new();
    private readonly List<Player> raffleWinners = new();
    private readonly List<Player> raffleLosers = new();
    private Convocatoria() { }
    private Convocatoria(ConvocatoriaId id, ConvocatoriaDate date, Observations observations)
    { Id = id; Date = date; Status = ConvocatoriaStatus.Pending; Observations = observations; }
    public ConvocatoriaId Id { get; private set; }
    public ConvocatoriaDate Date { get; private set; }
    public ConvocatoriaStatus Status { get; private set; }
    public Observations Observations { get; private set; }
    public IReadOnlyCollection<Player> CurrentPlayers => currentPlayers.AsReadOnly();
    public IReadOnlyCollection<Player> RaffleWinners => raffleWinners.AsReadOnly();
    public IReadOnlyCollection<Player> RaffleLosers => raffleLosers.AsReadOnly();
    public static Convocatoria Create(ConvocatoriaDate date, Observations? observations = null) => new(ConvocatoriaId.New(), date, observations ?? new Observations());
    public void UpdateDetails(ConvocatoriaDate date, Observations observations) { Date = date; Observations = observations; }
    public void SetStatus(ConvocatoriaStatus status) => Status = status;
    internal void AssignRaffle(IReadOnlyCollection<Player> winners, IReadOnlyCollection<Player> losers)
    {
        if (raffleWinners.Count > 0 || raffleLosers.Count > 0) throw new InvalidOperationException("Raffle results cannot be changed once assigned.");
        if (winners.Count == 0 && losers.Count == 0) throw new ArgumentException("Raffle results cannot be empty.");
        if (winners.Any(losers.Contains)) throw new ArgumentException("A player cannot be both winner and loser.");
        raffleWinners.AddRange(winners); raffleLosers.AddRange(losers); currentPlayers.AddRange(winners);
    }
}
