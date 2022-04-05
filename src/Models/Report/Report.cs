namespace BookkeeperRest.Models.Report;
using BookkeeperRest.Models.Earning;
using BookkeeperRest.Models.Expense;

[Serializable]
public record Report
{
    public DateTime Date { get; init; }
    public IEnumerable<EarningDTO> Earnings { get; init; }
    public IEnumerable<ExpenseDTO> Expenses { get; init; }
}