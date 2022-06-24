using BookkeeperRest.Models;

namespace BookkeeperRest.Repositories;

public interface IDenominationRepository
{
    public void RemoveAll();
    public void RemoveMultiple(IEnumerable<int> values);
    public void RemoveByValue(int value);
    public void Update(Denomination denomination);
    public void Add(Denomination denomination);
    public void Add(IEnumerable<Denomination> denominations);
    public IEnumerable<Denomination> GetAll();
}