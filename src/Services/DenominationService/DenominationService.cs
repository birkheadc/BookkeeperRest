using BookkeeperRest.New.Models;
using BookkeeperRest.New.Repositories;

namespace BookkeeperRest.New.Services;

public class DenominationService : IDenominationService
{

    private readonly IDenominationRepository denominationRepository;

    public DenominationService(IDenominationRepository denominationRepository)
    {
        this.denominationRepository = denominationRepository;
    }

    public void DeleteByValues(IEnumerable<int> values)
    {
        denominationRepository.DeleteByValues(values);
    }

    public IEnumerable<Denomination> GetAllDenominations()
    {
        return denominationRepository.GetAllDenominations();
    }

    public void UpdateAllDenominations(IEnumerable<Denomination> denominations)
    {
        denominationRepository.UpdateAll(denominations);
    }
}