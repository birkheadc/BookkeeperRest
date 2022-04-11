using BookkeeperRest.Models;
using BookkeeperRest.Models.Transaction;

namespace BookkeeperRest.Services;

public interface ITransactionTypeService
{
    public void RemoveAll();
    public void RemoveByName(string name);
    public void UpdateByName(string name, bool isDefault);
    public void Add(string name, bool isDefault = false);
    public void Add(TransactionType type);
    public IEnumerable<TransactionType> GetAll();
}