namespace BookkeeperRest.Models.Transaction;

public record NewTransactionDTO
{
    public DateTime Date { get; init; }
    public string Type { get; init; } = "";
    public long Amount { get; init; }
    public string Note { get; init; } = "";
}