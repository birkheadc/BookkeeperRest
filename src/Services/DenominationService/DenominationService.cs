using BookkeeperRest.Models;
using BookkeeperRest.Repositories;

namespace BookkeeperRest.Services;

public class DenominationService : IDenominationService
{
    private IDenominationRepository repository;

    public DenominationService(IDenominationRepository repository)
    {
        this.repository = repository;
    }

    public void Add(int value, bool isDefault = false)
    {
        Denomination denomination = new()
        {
            Value = value,
            IsDefault = isDefault
        };
        Add(denomination);
    }

    public void Add(Denomination denomination)
    {
        repository.Add(denomination);
    }

    public void Add(IEnumerable<Denomination> denominations)
    {
        repository.Add(denominations);
    }

    public IEnumerable<Denomination> GetAll()
    {
        return repository.GetAll();
    }

    public void RemoveAll()
    {
        repository.RemoveAll();
    }

    public void RemoveByValue(int value)
    {
        repository.RemoveByValue(value);
    }

    public void Update(Denomination denomination)
    {
        repository.Update(denomination);
    }
}