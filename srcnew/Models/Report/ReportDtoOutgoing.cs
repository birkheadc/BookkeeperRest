namespace BookkeeperRest.New.Models;

public class ReportDtoOutgoing
{
    public DateTime Date { get; init; }
    public IEnumerable<Earning> Earnings { get; init; }
    public IEnumerable<Expense> Expenses { get; init; }
    public IEnumerable<Category> EarningCategories { get; init; }
    public IEnumerable<Category> ExpenseCategories { get; init; }
    public IEnumerable<Denomination> Denominations { get; init; }
    public bool IsCashDefault { get; init; }
}