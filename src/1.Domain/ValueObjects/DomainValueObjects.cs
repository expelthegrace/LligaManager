namespace LligaManager.Domain.ValueObjects;

public readonly record struct PlayerId(Guid Value)
{
    public static PlayerId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
public readonly record struct ConvocatoriaId(Guid Value)
{
    public static ConvocatoriaId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
public readonly record struct PlayerName
{
    public string Value { get; }
    public PlayerName(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Player name is required.", nameof(value));
        Value = value.Trim();
    }
    public override string ToString() => Value;
}
public readonly record struct ConvocatoriaDate
{
    public DateOnly Value { get; }
    public ConvocatoriaDate(DateOnly value) => Value = value;
    public override string ToString() => Value.ToString("yyyy-MM-dd");
}
public readonly record struct Observations
{
    public string Value { get; }
    public Observations(string? value) => Value = value?.Trim() ?? string.Empty;
    public override string ToString() => Value;
}
