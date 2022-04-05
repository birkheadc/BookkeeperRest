namespace BookkeeperRest.Models.Expense;

[Serializable]
public record ExpenseDTO
{
    public string Type { get; init; } = "";
    public long Amount { get; init; }
    public string Note { get; init; } = "";
}