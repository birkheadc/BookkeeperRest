namespace BookkeeperRest.New.Models;

public record UserSettingsWrapper
{
    public UserSettings UserSettings { get; init; }
    public IEnumerable<Category> EarningCategories { get; init; }
    public IEnumerable<Category> ExpenseCategories { get; init; }
    public IEnumerable<Denomination> Denominations { get; init; }
}