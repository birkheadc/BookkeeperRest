namespace BookkeeperRest.Models.Transaction;

public record TransactionDTO
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public string Type { get; init; } = "";
    public long Amount { get; init; }
    public string Note { get; init; } = "";
}