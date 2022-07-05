using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Services;

public interface IDenominationService
{
    public IEnumerable<Denomination> GetAllDenominations();
    public void UpdateAllDenominations(IEnumerable<Denomination> denominations);
    public void DeleteByValues(IEnumerable<int> values);
}