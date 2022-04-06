using BookkeeperRest.Models.Report;
using BookkeeperRest.Models.Summary;
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

    public Summary BuildSummary(DateTime startDate, DateTime endDate)
    {
        long gross = 0;
        long net = 0;

        List<Transaction> posTransactions = new();
        List<Transaction> negTransactions = new();

        TimeSpan span = endDate.Subtract(startDate);
        int days = (int)Math.Floor(span.TotalDays);

        for (int i = 0; i <= days; i++)
        {
            List<Transaction> transactions = new();
            transactions.AddRange(repository.FindByDate(startDate.AddDays(i)));

            foreach (Transaction transaction in transactions)
            {
                net += transaction.Amount;
                if (transaction.Amount > 0)
                {
                    posTransactions.Add(transaction);
                    gross += transaction.Amount;
                    continue;
                }
                negTransactions.Add(transaction);
            }
        }
        
        Summary summary = new()
        {
            StartDate = startDate,
            EndDate = endDate,
            Gross = gross,
            Net = net,
            PositiveTransactions = converter.ToDTO(posTransactions),
            NegativeTransactions = converter.ToDTO(negTransactions)
        };
        
        return summary;
    }

    public IEnumerable<TransactionDTO> GetAllTransactionsNewestFirst()
    {
        return converter.ToDTO(repository.FindAllOrderByDateDesc());
    }

    public void HandleReport(Report report)
    {
        if (report.Transactions is not null)
        {
            repository.Add(converter.ToEntity(report.Transactions));
        }
    }
}