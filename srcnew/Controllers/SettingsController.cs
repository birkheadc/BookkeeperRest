using BookkeeperRest.New.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.New.Controllers;

[ApiController]
[Route("api/new/settings")]
[PasswordAuth]
public class SettingsController : ControllerBase
{
    
}