using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Repositories;

public interface IUserSettingRepository
{
    public string GetValueByName(string name);
    public UserSettings GetAllSettings();
    public void UpdateSetting(IEnumerable<UserSetting> settings);
}