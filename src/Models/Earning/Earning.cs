namespace BookkeeperRest.Models.Earning;

[Serializable]
public record Earning
{
    public DateTime Date { get; init; }
    public string Type { get; init; } = "";
    public long Amount { get; init; }
    public string Note { get; init; } = "";
}