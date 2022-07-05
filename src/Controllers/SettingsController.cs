using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookkeeperRest.New.Models;
using BookkeeperRest.New.Filters;
using BookkeeperRest.New.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.New.Controllers;

[ApiController]
[Route("api/settings")]
[PasswordAuth]
public class SettingsController : ControllerBase
{

    private readonly IUserSettingService userSettingService;

    public SettingsController(IUserSettingService userSettingService)
    {
        this.userSettingService = userSettingService;
    }

    [HttpGet]
    public IActionResult GetAllSettings()
    {
        try
        {
            UserSettingsWrapper wrapper = userSettingService.GetAllSettings();
            return Ok(wrapper);
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    [HttpGet]
    [Route("name")]
    public IActionResult GetValueByName([FromQuery(Name = "name"), Required] string name)
    {
        try
        {
            return Ok(userSettingService.GetValueByName(name));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound("A setting with that name does not exist.");
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    [HttpPut]
    public IActionResult UpdateSettings([FromBody, Required] JsonElement element)
    {
        try
        {
            IEnumerable<UserSetting> userSettings = ParseJsonForUserSettings(element);
            userSettingService.UpdateSettings(userSettings);
            return Ok();
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    private IEnumerable<UserSetting> ParseJsonForUserSettings(JsonElement element)
    {
        Dictionary<string, string> dic = element.Deserialize<Dictionary<string, string>>();
        List<UserSetting> userSettings = new();
        foreach (KeyValuePair<string, string> item in dic)
        {
            UserSetting userSetting = new()
            {
                Name = item.Key,
                Value = item.Value
            };
            userSettings.Add(userSetting);
        }
        return userSettings;
    }
}