using BookkeeperRest.Repositories;

namespace BookkeeperRest.Services;

public class DenominationService : IDenominationService
{
    private IDenominationRepository repository;

    public DenominationService(IDenominationRepository repository)
    {
        this.repository = repository;
    }
}