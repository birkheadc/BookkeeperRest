using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BookkeeperRest.New.Filters;
using BookkeeperRest.New.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.New.Controllers;

[ApiController]
[Route("api/password")]
[PasswordAuth]
public class PasswordController : ControllerBase
{
    private IPasswordService passwordService;

    public PasswordController(IPasswordService passwordService)
    {
        this.passwordService = passwordService;
    }

    [HttpPost]
    public IActionResult ChangePassword([FromBody] JsonElement json)
    {
        try
        {
            if (!json.TryGetProperty("password", out var password))
            {
                return BadRequest("`password` field is required.");
            }
            passwordService.ChangePassword(password.ToString());
            return Ok();
        }
        catch (ArgumentException e)
        {
            return BadRequest("Password is not valid!");
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    [HttpPost]
    [Route("verify")]
    public IActionResult VerifyPassword()
    {
        return Ok();
    }
}