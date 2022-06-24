namespace BookkeeperRest.Models.Transaction;

public record Transaction
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public string Type { get; init; } = "";
    public long Amount { get; init; }
    public string Note { get; init; } = "";

    public override string ToString()
    {
        return Date.ToShortDateString() + "," + Type + "," + Amount.ToString() + "," + Note;
    }
}