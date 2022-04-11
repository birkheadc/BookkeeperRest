using BookkeeperRest.Models;

namespace BookkeeperRest.Repositories;

public interface IDenominationRepository
{
    public void RemoveAll();
    public void RemoveByValue(int value);
    public void UpdateByValue(int value, bool isDefault);
    public void Add(Denomination denomination);
    public IEnumerable<Denomination> GetAll();
}