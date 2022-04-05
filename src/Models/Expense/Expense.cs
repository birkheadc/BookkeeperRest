namespace BookkeeperRest.Models.Expense;

[Serializable]
public record Expense
{
    public DateTime Date { get; init; }
    public string Type { get; init; } = "";
    public long Amount { get; init; }
    public string Note { get; init; } = "";
}