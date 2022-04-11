using BookkeeperRest.Repositories;

namespace BookkeeperRest.Services;

public class TransactionTypeService : ITransactionTypeService
{
    private ITransactionTypeRepository repository;

    public TransactionTypeService(ITransactionTypeRepository repository)
    {
        this.repository = repository;
    }
}