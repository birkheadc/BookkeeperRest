namespace BookkeeperRest.New.Models;

public record CategoriesWrapper
{
    public IEnumerable<Category> EarningCategories { get; init; }
    public IEnumerable<Category> ExpenseCategories { get; init; }
}