using BookkeeperRest.Models.Report;
using BookkeeperRest.Models.Transaction;

namespace BookkeeperRest.Services.TransactionService;

public interface ITransactionService
{
    public IEnumerable<TransactionDTO> GetAllTransactionsNewestFirst();
    public void HandleReport(Report report);
}