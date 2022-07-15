namespace BookkeeperRest.New.Models;
public record ReportDtoIncoming
{
    public DateTime Date { get; init; }
    public IEnumerable<EarningDtoIncoming> Earnings { get; init; }
    public IEnumerable<ExpenseDtoIncoming> Expenses { get; init; }
}