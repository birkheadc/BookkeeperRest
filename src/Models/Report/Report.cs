using BookkeeperRest.Models.Transaction;

namespace BookkeeperRest.Models.Report;

public record Report
{
    public IEnumerable<NewTransactionDTO> ?Transactions { get; init; }
}