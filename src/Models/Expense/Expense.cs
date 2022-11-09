namespace BookkeeperRest.New.Models;

public record Expense
{
    public Guid Id { get; init; }
    public string Category { get; init; }
    public long Amount { get; init; }
    public DateTime Date { get; init; }
    public string Note { get; init; } = "";
    public bool WasTakenFromCash { get; init; }

    public override string ToString()
    {
        return Date.ToShortDateString() + "," + Category + "," + (Amount * -1).ToString() + "," + Note + "," + WasTakenFromCash.ToString();
    }
}