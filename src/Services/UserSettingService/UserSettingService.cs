using BookkeeperRest.New.Models;
using BookkeeperRest.New.Repositories;

namespace BookkeeperRest.New.Services;

public class UserSettingService : IUserSettingService
{
    private readonly IUserSettingRepository userSettingRepository;
    private readonly IEarningCategoryRepository earningCategoryRepository;
    private readonly IExpenseCategoryRepository expenseCategoryRepository;
    private readonly IDenominationRepository denominationRepository;

    public UserSettingService(IUserSettingRepository userSettingRepository, IEarningCategoryRepository earningCategoryRepository, IExpenseCategoryRepository expenseCategoryRepository, IDenominationRepository denominationRepository)
    {
        this.userSettingRepository = userSettingRepository;
        this.earningCategoryRepository = earningCategoryRepository;
        this.expenseCategoryRepository = expenseCategoryRepository;
        this.denominationRepository = denominationRepository;
    }

    public UserSettingsWrapper GetAllSettings()
    {
        UserSettingsWrapper wrapper = new()
        {
            UserSettings = userSettingRepository.GetAllSettings(),
            EarningCategories = earningCategoryRepository.GetAllCategories(),
            ExpenseCategories = expenseCategoryRepository.GetAllCategories(),
            Denominations = denominationRepository.GetAllDenominations()
        };
        return wrapper;
    }

    public string GetValueByName(string name)
    {
        return userSettingRepository.GetValueByName(name);
    }

    public void UpdateSettings(UserSettingsWrapper wrapper)
    {
        // Todo (To redo)
        userSettingRepository.UpdateSettings(wrapper.UserSettings);
        earningCategoryRepository.UpdateCategories(wrapper.EarningCategories);
        expenseCategoryRepository.UpdateCategories(wrapper.ExpenseCategories);
        denominationRepository.UpdateAll(wrapper.Denominations);
    }   
}