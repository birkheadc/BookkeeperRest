using BookkeeperRest.Filters;
using BookkeeperRest.Models;
using BookkeeperRest.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.Controllers;

[ApiController]
[Route("setting")]
[PasswordAuth]
public class SettingController : ControllerBase
{
    private readonly ISettingService service;

    public SettingController(ISettingService service)
    {
        this.service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(service.GetAll());
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult Add([FromBody] Setting setting)
    {
        try
        {
            service.Add(setting);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    [Route("{key}")]
    public IActionResult DeleteByKey([FromRoute] string key)
    {
        try
        {
            service.DeleteByKey(key);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut]
    public IActionResult UpdateByKey([FromBody] Setting setting)
    {
        try
        {
            service.UpdateByKey(setting.Key, setting.Value);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}