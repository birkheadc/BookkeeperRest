using BookkeeperRest.Models;
using BookkeeperRest.Models.Transaction;
using BookkeeperRest.Repositories;

namespace BookkeeperRest.Services;

public class TransactionTypeService : ITransactionTypeService
{
    private ITransactionTypeRepository repository;
    private ITransactionConverter converter;

    public TransactionTypeService(ITransactionTypeRepository repository, ITransactionConverter converter)
    {
        this.repository = repository;
        this.converter = converter;
    }

    public void Add(string name, bool isDefault = false)
    {
        if (IsNameValid(name) == false) {
            throw new ArgumentException();
        }
        TransactionType type = new()
        {
            Name = ConvertName(name),
            IsDefault = isDefault
        };
        Add(type);
    }

    public void Add(TransactionType type)
    {
        if (IsNameValid(type.Name) == false) {
            throw new ArgumentException();
        }
        repository.Add(ConvertTransactionTypeName(type));
    }

    public void Add(IEnumerable<TransactionType> types) {
        foreach (TransactionType type in types) {
            if (IsNameValid(type.Name) == false) {
                throw new ArgumentException();
            }
        }
        List<TransactionType> newTypes = new();
        foreach (TransactionType type in types) {
            newTypes.Add(ConvertTransactionTypeName(type));
        }
        repository.Add(newTypes);
    }

    private bool IsNameValid(string name) {
        return converter.IsNameValid(name);
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

    private string ConvertName(string name)
    {
        return converter.ConvertName(name);
    }

    private TransactionType ConvertTransactionTypeName(TransactionType type)
    {
        string newName = ConvertName(type.Name);
        TransactionType newType = new()
        {
            Name = newName,
            Polarity = type.Polarity,
            IsDefault = type.IsDefault
        };
        return newType;
    }
}