using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Repositories;

public interface IEarningCategoryRepository
{
    public void AddCategoriesByEarnings(IEnumerable<Earning> earnings);
    public void UpdateCategories(IEnumerable<Category> categories);
    public IEnumerable<Category> GetAllCategories();
    public void DeleteByNames(IEnumerable<string> names);
}