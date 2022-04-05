using BookkeeperRest.Models.Report;
using BookkeeperRest.Models.Transaction;
using BookkeeperRest.Repositories.TransactionRepository;

namespace BookkeeperRest.Services.TransactionService;

public class TransactionService : ITransactionService
{
    private readonly ITransactionConverter converter;
    private readonly ITransactionRepository repository;

    public TransactionService(ITransactionConverter converter, ITransactionRepository repository)
    {
        this.converter = converter;
        this.repository = repository;
    }

    public IEnumerable<TransactionDTO> GetAllTransactionsNewestFirst()
    {
        return converter.ToDTO(repository.FindAllByDateDesc());
    }

    public void HandleReport(Report report)
    {
        if (report.Transactions is not null)
        {
            repository.Add(converter.ToEntity(report.Transactions));
        }
    }
}