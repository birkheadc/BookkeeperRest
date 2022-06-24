using BookkeeperRest.New.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.New.Controllers;

[ApiController]
[Route("api/new/password")]
[PasswordAuth]
public class PasswordController : ControllerBase
{
    
}