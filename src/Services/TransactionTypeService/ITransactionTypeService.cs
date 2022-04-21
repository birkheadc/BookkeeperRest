using BookkeeperRest.Models;
using BookkeeperRest.Models.Transaction;

namespace BookkeeperRest.Services;

public interface ITransactionTypeService
{
    public void RemoveAll();
    public void RemoveMultiple(IEnumerable<string> names);
    public void RemoveByName(string name);
    public void Update(TransactionType transactionType);
    public void Add(string name, bool isDefault = false);
    public void Add(TransactionType type);
    public void Add(IEnumerable<TransactionType> types);
    public IEnumerable<TransactionType> GetAll();
}