using BookkeeperRest.Models.Transaction;

namespace BookkeeperRest.Models.Report;

public record UpdateReport
{
    public IEnumerable<TransactionDTO> ?Transactions { get; init; }
}