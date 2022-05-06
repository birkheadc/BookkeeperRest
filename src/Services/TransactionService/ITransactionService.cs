using BookkeeperRest.Models.Report;
using BookkeeperRest.Models.Summary;
using BookkeeperRest.Models.Transaction;
using BookkeeperREst.Models.Csv;

namespace BookkeeperRest.Services.TransactionService;

public interface ITransactionService
{
    public Summary BuildSummary(DateTime startDate, DateTime endDate);
    public IEnumerable<TransactionDTO> GetAllTransactionsNewestFirst();
    public void HandleReport(Report report);
    public void DeleteById(string id);
    public void UpdateMultiple(UpdateReport report);
    public IEnumerable<Summary> BuildSummariesForPastNDays(int n);
    public void RecordCsv(CsvDto csv);
}