using System.Text.Json;
using BookkeeperRest.Filters;
using BookkeeperRest.Services.PasswordService;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.OpenSsl;

namespace BookkeeperRest.Controllers;

[ApiController]
[Route("password")]
[PasswordAuth]
public class PasswordController : ControllerBase
{
    private IPasswordService service;

    public PasswordController(IPasswordService service)
    {
        this.service = service;
    }

    [HttpPost]
    public IActionResult ChangePassword([FromBody] JsonElement element)
    {
        if (!element.TryGetProperty("old", out var oldElement) || !element.TryGetProperty("new", out var newElement))
        {
            return BadRequest();
        }

        string oldPassword = oldElement.GetString() ?? "";
        string newPassword = newElement.GetString() ?? "";

        if (string.IsNullOrWhiteSpace(newPassword))
        {
            return BadRequest();
        }

        if (service.ChangePassword(oldPassword, newPassword))
        {
            return Ok();
        }      

        return Forbid();
    }

    [HttpPost]
    [Route("verify")]
    public IActionResult VerifyPassword()
    {
        return Ok();    
    }
}