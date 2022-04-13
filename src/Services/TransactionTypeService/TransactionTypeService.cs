using BookkeeperRest.Models;
using BookkeeperRest.Repositories;

namespace BookkeeperRest.Services;

public class TransactionTypeService : ITransactionTypeService
{
    private ITransactionTypeRepository repository;

    public TransactionTypeService(ITransactionTypeRepository repository)
    {
        this.repository = repository;
    }

    public void Add(string name, bool isDefault = false)
    {
        TransactionType type = new()
        {
            Name = name,
            IsDefault = isDefault
        };
        Add(type);
    }

    public void Add(TransactionType type)
    {
        if (IsNameValid(type.Name) == false) {
            throw new ArgumentException();
        }
        repository.Add(type);
    }

    private bool IsNameValid(string name) {
        foreach (char c in name) {
            if (Char.IsLetter(c)) {
                continue;
            }
            if (c != '_' && c != ' ' && c != '-') {
                return false;
            }
        }
        return true;
    }

    public IEnumerable<TransactionType> GetAll()
    {
        return repository.GetAll();
    }

    public void RemoveAll()
    {
        repository.RemoveAll();
    }

    public void RemoveByName(string name)
    {
        repository.RemoveByName(name);
    }

    public void UpdateByName(string name, bool isDefault)
    {
        repository.UpdateByName(name, isDefault);
    }
}