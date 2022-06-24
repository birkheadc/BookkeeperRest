namespace BookkeeperRest.New.Models;

public class ReportDtoIncoming
{
    public DateTime Date { get; init; }
    public IEnumerable<Earning> Earnings { get; init; }
    public IEnumerable<Expense> Expenses { get; init; }
}