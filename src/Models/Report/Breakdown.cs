namespace BookkeeperRest.New.Models;

public record Breakdown
{
    public string Category { get; init; }
    public long Total { get; init; }
    public double Average { get; init; }
}