using System.Text;
using BookkeeperRest.Email;
using BookkeeperRest.Models.Report;
using BookkeeperRest.Models.Summary;
using BookkeeperRest.Models.Transaction;
using BookkeeperRest.Repositories.TransactionRepository;
using BookkeeperRest.Models.Csv;

namespace BookkeeperRest.Services.TransactionService;

public class TransactionService : ITransactionService
{
    private readonly ITransactionConverter converter;
    private readonly ITransactionRepository repository;
    private readonly IEmailSender emailSender;

    public TransactionService(ITransactionConverter converter, ITransactionRepository repository, IEmailSender emailSender)
    {
        this.converter = converter;
        this.repository = repository;
        this.emailSender = emailSender;
    }

    public Summary BuildSummary(DateTime startDate, DateTime endDate)
    {
        long gross = 0;
        long net = 0;

        List<Transaction> transactions = new();
        transactions.AddRange(repository.FindBetweenDates(startDate, endDate));

        List<Transaction> posTransactions = new();
        List<Transaction> negTransactions = new();


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

    public IEnumerable<Summary> BuildSummariesForPastNDays(int n)
    {
        DateTime endDate = DateTime.Today;
        DateTime startDate = endDate.Subtract(TimeSpan.FromDays(n - 1));

        List<Summary> summaries = new();

        for (int i = 0; i < n; i++)
        {
            DateTime date = startDate.Add(TimeSpan.FromDays(i));
            Summary summary = BuildSummary(date, date);
            summaries.Add(summary);
        }
        return summaries;
    }

    public void DeleteById(string id)
    {
        repository.DeleteById(id);
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
        SendUpdateEmail();
    }

    public void UpdateMultiple(UpdateReport report)
    {
        if (report.Transactions is not null) {
            repository.UpdateMultiple(converter.ToEntity(report.Transactions));
        }
    }

    public void RecordCsv(CsvDto csv)
    {
        IFormFile file = csv.File;
        if (file is null || file.Length == 0)
        {
            throw new ArgumentNullException();
        }
        List<Transaction> transactions = new();
        using (StreamReader reader = new StreamReader(file.OpenReadStream()))
        {
            List<string> transactionTypes = GetTransactionTypesFromHeader(reader.ReadLine());
            while (!reader.EndOfStream)
            {
                transactions.AddRange(GetTransactionsFromLine(reader.ReadLine(), transactionTypes));
            }
        }

        repository.Add(transactions);
    }

    private List<string> GetTransactionTypesFromHeader(string? header)
    {
        // Null check
        List<string> headers = new();
        if (header is null)
        {
            return headers;
        }

        // Get each Value from the line of Comma Separated Values
        string[] strings = header.Split(',');

        // Don't skip date / total lines, since the next method needs them there for the indexes to line up properly.
        for (int i = 0; i < strings.Length; i++)
        {
            headers.Add(strings[i]);
        }

        return headers;
    }

    private List<Transaction> GetTransactionsFromLine(string? line, List<string> transactionTypes)
    {
        // Null check
        List<Transaction> transactions = new();
        if (line is null)
        {
            return transactions;
        }

        // Get each Value from the line of Comma Separated Values
        string[] values = line.Split(',');

        DateTime date = DateTime.Parse(values[0]);

        // We have to build up one big cash transaction based on which transactions were paid for out of the register
        // (In this case, any transaction which does not have an asterisk at the end of the name)
        long cash = 0;
        
        // First element is Date, so skip it. Last element is Total, so skip it as well, since we don't record that any more.
        for (int i = 1; i < values.Length - 1; i++)
        {
            // If the value is zero there is nothing to record.
            if (Int64.Parse(values[i]) == 0)
            {
                continue;
            }

            Transaction transaction;
            // Any transaction called 'cash' or not ending in '*' (except card and accounttransfer) needs to add it's value to the cash transaction
            if (transactionTypes[i] == "cash" || (transactionTypes[i].Last() != '*' && transactionTypes[i] != "card" && transactionTypes[i] != "accounttransfer"))
            {
                cash += Int64.Parse(values[i]);
            }
            if (transactionTypes[i] == "cash")
            {
                continue;
            }
            // Old format did not do a good job of categorizing earning vs expenses, so this is hard coded.
            if (transactionTypes[i] == "card" || transactionTypes[i] == "accounttransfer")
            {
                // These will be earnings, ie positive transactions
                string type = transactionTypes[i].Replace("*", "");
                // Just doing this because I don't like how my wife named it...
                if (type == "accounttransfer")
                {
                    type = "direct_deposit";
                }
                transaction = new()
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    Type = type,
                    Amount = Int64.Parse(values[i]),
                    // The old format did not support notes, so there will be no note
                    Note = ""
                };
                transactions.Add(transaction);
                continue;
            }
            // Anything else will be an expense, ie negative transaction. Amount should be made negative
            transaction = new()
            {
                Id = Guid.NewGuid(),
                Date = date,
                Type = transactionTypes[i].Replace("*", ""),
                Amount = Int64.Parse(values[i]) * -1,
                // The old format did not support notes, so there will be no note
                Note = ""
            };
            transactions.Add(transaction);
        }

        if (cash > 0)
        {
            Transaction cashTransaction = new()
            {
                Id = Guid.NewGuid(),
                Date = date,
                Type = "cash",
                Amount = cash,
                Note = ""
            };
            transactions.Add(cashTransaction);
        }
        
        return transactions;
    }
    
    private string BuildCsvSummary()
    {
        List<Transaction> transactions = new List<Transaction>();
        transactions.AddRange(repository.FindAllOrderByDateDesc());

        StringBuilder sb = new();

        foreach (Transaction transaction in transactions)
        {
            sb.Append(transaction.ToString() + "\n");
        }

        return sb.ToString();
    }

    private string BuildCsvSummary(DateTime startDate, DateTime endDate)
    {
        return "not yet implemented";
    }

    private void SendUpdateEmail()
    {
        Console.WriteLine("Attempting to send email...");
        string attachmentFileName = "Bookkeeper_Backup_" + DateTime.Now.ToString() + ".txt";
        string attachmentContent = BuildCsvSummary();
        Console.WriteLine(attachmentFileName);
        SimpleTextAttachment attachment = new()
        {
            FileName = attachmentFileName,
            Content = attachmentContent
        };
        EmailMessage message = new("Colby", "birkheadc@gmail.com", "Bookkeeper Updated", "Attached is a back up of all transactions as of the recent addition.", attachment);
        emailSender.SendEmailAsync(message);
    }
}