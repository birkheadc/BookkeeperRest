namespace BookkeeperRest.New.Models;

public record ReportsWrapper
{
    public IEnumerable<Report> Reports { get; init; }
    public IEnumerable<Category> EarningCategories { get; init; }
    public IEnumerable<Category> ExpenseCategories { get; init; }
    public IEnumerable<Denomination> Denominations { get; init; }
    public Summary Summary { get; init; }
}