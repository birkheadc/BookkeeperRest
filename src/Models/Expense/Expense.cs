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
        return "Expense Id=" + Id + " Category=" + Category + " Amount=" + Amount + " Date=" + Date.ToShortDateString() + " Note=" + Note + " Was Taken From Cash=" + WasTakenFromCash;
    }
}