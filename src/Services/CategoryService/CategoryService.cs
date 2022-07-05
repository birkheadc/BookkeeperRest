using BookkeeperRest.New.Models;
using BookkeeperRest.New.Repositories;

namespace BookkeeperRest.New.Services;

public class CategoryService : ICategoryService
{
    private readonly IEarningCategoryRepository earningCategoryRepository;
    private readonly IExpenseCategoryRepository expenseCategoryRepository;

    public CategoryService(IExpenseCategoryRepository expenseCategoryRepository, IEarningCategoryRepository earningCategoryRepository)
    {
        this.expenseCategoryRepository = expenseCategoryRepository;
        this.earningCategoryRepository = earningCategoryRepository;
    }

    public void UpdateAllCategories(CategoriesWrapper categories)
    {
        earningCategoryRepository.UpdateCategories(categories.EarningCategories);
        expenseCategoryRepository.UpdateCategories(categories.ExpenseCategories);
    }

    public CategoriesWrapper GetAllCategories()
    {
        CategoriesWrapper wrapper = new()
        {
            EarningCategories = earningCategoryRepository.GetAllCategories(),
            ExpenseCategories = expenseCategoryRepository.GetAllCategories()
        };
        return wrapper;
    }
}