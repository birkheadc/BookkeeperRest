using BookkeeperRest.Models;

namespace BookkeeperRest.Services;

public interface ISettingService
{
    public string GetAll();
    public void Add(string key, string value);
    public void Add(Setting setting);
    public void UpdateByKey(string key, string value);
    public void DeleteByKey(string key);
}