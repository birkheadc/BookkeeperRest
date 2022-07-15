namespace BookkeeperRest.New.Models;

public record Earning
{
    public Guid Id { get; init; }
    public string Category { get; init; }
    public long Amount { get; init; }
    public DateTime Date { get; init; }

    public override string ToString()
    {
        return "Earning Id=" + Id + " Category=" + Category + " Amount=" + Amount + " Date=" + Date.ToShortDateString();
    }
}