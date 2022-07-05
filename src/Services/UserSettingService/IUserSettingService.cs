using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Services;

public interface IUserSettingService
{
    public UserSettingsWrapper GetAllSettings();
    public string GetValueByName(string name);
    public void UpdateSettings(IEnumerable<UserSetting> settings);
}