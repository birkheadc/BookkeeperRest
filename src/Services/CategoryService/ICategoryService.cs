using BookkeeperRest.New.Models;
using BookkeeperRest.New.Repositories;

namespace BookkeeperRest.New.Services;

public interface ICategoryService
{
    public void UpdateAllCategories(CategoriesWrapper categories);
    public CategoriesWrapper GetAllCategories();
}