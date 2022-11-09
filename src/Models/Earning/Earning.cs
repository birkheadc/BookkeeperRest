namespace BookkeeperRest.New.Models;

public record Earning
{
    public Guid Id { get; init; }
    public string Category { get; init; }
    public long Amount { get; init; }
    public DateTime Date { get; init; }

    public override string ToString()
    {
        return Date.ToShortDateString() + "," + Category + "," + Amount.ToString();
    }
}