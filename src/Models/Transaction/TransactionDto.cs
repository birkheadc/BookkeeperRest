namespace BookkeeperRest.New.Models;

public record TransactionDto
{
    public string Category { get; init; }
    public long Amount { get; init; }
    public DateTime Date { get; init; }
    public string Note { get; init; } = "";
    public bool WasTakenFromCash { get; init; }

}