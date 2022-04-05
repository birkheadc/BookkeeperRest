namespace BookkeeperRest.Models.Earning;

[Serializable]
public record EarningDTO
{
    public string Type { get; init; } = "";
    public long Amount { get; init; }
    public string Note { get; init; } = "";
}