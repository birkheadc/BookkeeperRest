using BookkeeperRest.Models.Transaction;

namespace BookkeeperRest.Models.Report;

public class Report
{
    public IEnumerable<NewTransactionDTO> ?Transactions { get; init; }
}