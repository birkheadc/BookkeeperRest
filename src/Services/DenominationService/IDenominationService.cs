using BookkeeperRest.Models;

namespace BookkeeperRest.Services;

public interface IDenominationService
{
    public void RemoveAll();
    public void RemoveByValue(int value);
    public void UpdateByValue(int value, bool isDefault);
    public void Add(int value, bool isDefault = false);
    public IEnumerable<Denomination> GetAll();
}