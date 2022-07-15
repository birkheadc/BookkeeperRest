using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Repositories;

public interface IExpenseCategoryRepository
{
    public void AddCategoriesByExpenses(IEnumerable<Expense> expenses);
    public void UpdateCategories(IEnumerable<Category> categories);
    public IEnumerable<Category> GetAllCategories();
    public void DeleteByNames(IEnumerable<string> names);
}