using BookkeeperRest.Models.Transaction;

namespace BookkeeperRest.Repositories.TransactionRepository;

public interface ITransactionRepository
{
    public void Add(Transaction transaction);
    public void Add(IEnumerable<Transaction> transactions);
    public IEnumerable<Transaction> FindAllOrderByDateDesc();
    public IEnumerable<Transaction> FindByDate(DateTime date);
}