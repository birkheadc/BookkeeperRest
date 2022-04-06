using BookkeeperRest.Models.Transaction;

namespace BookkeeperRest.Models.Summary;

public record Summary
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public long Gross { get; init; }
    public long Net { get; init; }
    public IEnumerable<TransactionDTO> ?PositiveTransactions { get; init; }
    public IEnumerable<TransactionDTO> ?NegativeTransactions { get; init; }
}