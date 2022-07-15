using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Repositories;

public interface IDenominationRepository
{
    public IEnumerable<Denomination> GetAllDenominations();
    public void UpdateAll(IEnumerable<Denomination> denominations);
    public void DeleteByValues(IEnumerable<int> values);
}