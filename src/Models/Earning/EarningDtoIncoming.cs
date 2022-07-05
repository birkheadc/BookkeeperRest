namespace BookkeeperRest.New.Models;

public record EarningDtoIncoming
{
    public string Category { get; init; }
    public long Amount { get; init; }
    public DateTime Date { get; init; }

    public override string ToString()
    {
        return "Category=" + Category + " Amount=" + Amount + " Date=" + Date.ToShortDateString();
    }
}