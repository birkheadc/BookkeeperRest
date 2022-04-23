using BookkeeperRest.Models.Transaction;

namespace BookkeeperRest.Repositories.TransactionRepository;

public interface ITransactionRepository
{
    public void Add(Transaction transaction);
    public void Add(IEnumerable<Transaction> transactions);
    public IEnumerable<Transaction> FindAllOrderByDateDesc();
    public IEnumerable<Transaction> FindByDate(DateTime date);
    public IEnumerable<Transaction> FindBetweenDates(DateTime startDate, DateTime endDate);
    public void DeleteById(string id);
    public void UpdateMultiple(IEnumerable<Transaction> transactions);
}