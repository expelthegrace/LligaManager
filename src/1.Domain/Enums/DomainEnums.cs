namespace LligaManager.Domain.Enums;

public enum PlayerStatus { Available, Unavailable, Removed }
public enum PlayerPriority { Normal, High }
public enum ConvocatoriaStatus { Pending, Played, Descanso }
[Flags]
public enum PlayerRole { None = 0, Interior = 1, Exterior = 2, Base = 4 }
