namespace BookkeeperRest.New.Models;

public record Summary
{
    public long Gross { get; init; }
    public long Net { get; init; }
    public double AveGross { get; init; }
    public double AveNet { get; init; }
    public IEnumerable<Breakdown> Breakdowns { get; init; }
}