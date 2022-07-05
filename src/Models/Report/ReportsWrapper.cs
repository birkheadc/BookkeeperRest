namespace BookkeeperRest.New.Models;

public record ReportsWrapper
{
    public IEnumerable<Report> Reports { get; init; }
    public IEnumerable<Category> EarningCategories { get; init; }
    public IEnumerable<Category> ExpenseCategories { get; init; }
    public IEnumerable<Denomination> Denominations { get; init; }

    // IsCashDefault is a string because UserSettings only contains strings, and the back-end only passes and stores UserSettings as-is.
    //It is up to the front-end to correctly store and validate these settings. 
    public string IsCashDefault { get; init; }
}