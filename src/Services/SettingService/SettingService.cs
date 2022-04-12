using BookkeeperRest.Models;
using BookkeeperRest.Repositories;

namespace BookkeeperRest.Services;

public class SettingService : ISettingService
{
    private readonly ISettingRepository repository;

    public SettingService(ISettingRepository repository)
    {
        this.repository = repository;
    }

    public void Add(string key, string value)
    {
        Setting setting = new()
        {
            Key = key,
            Value = value
        };
        Add(setting);
    }

    public void Add(Setting setting)
    {
        repository.Add(setting);
    }

    public void DeleteByKey(string key)
    {
        repository.DeleteByKey(key);
    }

    public IEnumerable<Setting> GetAll()
    {
        return repository.GetAll();
    }

    public void UpdateByKey(string key, string value)
    {
        repository.UpdateByKey(key, value);
    }
}