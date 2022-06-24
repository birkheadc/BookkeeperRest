using BookkeeperRest.Models;

namespace BookkeeperRest.Repositories;

public interface ISettingRepository
{
    public IEnumerable<Setting> GetAll();
    public void Add(Setting setting);
    public void UpdateByKey(string key, string value);
    public void DeleteByKey(string key);
    public void UpdateAll(IEnumerable<Setting> settings);
}