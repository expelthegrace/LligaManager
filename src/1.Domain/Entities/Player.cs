using LligaManager.Domain.Enums;
using LligaManager.Domain.ValueObjects;

namespace LligaManager.Domain.Entities;

public sealed class Player
{
    private Player() { }
    private Player(PlayerId id, PlayerName name, PlayerRole roles, PlayerStatus status, PlayerPriority priority, Observations observations)
    {
        Id = id; Name = name; Roles = roles; Status = status; Priority = priority; Observations = observations;
    }
    public PlayerId Id { get; private set; }
    public PlayerName Name { get; private set; }
    public PlayerRole Roles { get; private set; }
    public PlayerStatus Status { get; private set; }
    public PlayerPriority Priority { get; private set; }
    public Observations Observations { get; private set; }
    public static Player Create(PlayerName name, PlayerRole roles, PlayerStatus status = PlayerStatus.Available, PlayerPriority priority = PlayerPriority.Normal, Observations? observations = null)
    {
        if (roles == PlayerRole.None) throw new ArgumentException("At least one player role is required.", nameof(roles));
        if (status == PlayerStatus.Removed) throw new ArgumentException("A new player cannot be removed.", nameof(status));
        return new(PlayerId.New(), name, roles, status, priority, observations ?? new Observations());
    }
    public void UpdateDetails(PlayerName name, PlayerRole roles, Observations observations)
    {
        if (roles == PlayerRole.None) throw new ArgumentException("At least one player role is required.", nameof(roles));
        Name = name; Roles = roles; Observations = observations;
    }
    public void SetStatus(PlayerStatus status) => Status = status;
    public void SetPriority(PlayerPriority priority) => Priority = priority;
}
