using System.Net.Mime;
using BookkeeperRest.Models;
using BookkeeperRest.Repositories;
using System.Text.Json;

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

    public string GetAll()
    {
        IEnumerable<Setting> settings = repository.GetAll();
        string json = ConvertSettingsToJsonString(settings);
        return json;
    }

    private string ConvertSettingsToJsonString(IEnumerable<Setting> settings)
    {
        Dictionary<string, string> pairs = new();
        foreach (Setting setting in settings)
        {
            pairs.Add(setting.Key, setting.Value);
        }
        string json = JsonSerializer.Serialize(pairs);
        return json;
    }

    public void UpdateByKey(string key, string value)
    {
        repository.UpdateByKey(key, value);
    }
}