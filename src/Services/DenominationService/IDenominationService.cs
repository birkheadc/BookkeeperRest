using BookkeeperRest.Models;

namespace BookkeeperRest.Services;

public interface IDenominationService
{
    public void RemoveAll();
    public void RemoveMultiple(IEnumerable<int> values);
    public void RemoveByValue(int value);
    public void Update(Denomination denomination);
    public void Add(int value, bool isDefault = false);
    public void Add(Denomination denomination);
    public void Add(IEnumerable<Denomination> denominations);
    public IEnumerable<Denomination> GetAll();
}