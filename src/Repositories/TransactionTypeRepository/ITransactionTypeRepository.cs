using BookkeeperRest.Models;

namespace BookkeeperRest.Repositories;

public interface ITransactionTypeRepository
{
    public void RemoveAll();
    public void RemoveByName(string name);
    public void Update(TransactionType transactionType);
    public void Add(TransactionType denomination);
    public void Add(IEnumerable<TransactionType> types);
    public IEnumerable<TransactionType> GetAll();
}