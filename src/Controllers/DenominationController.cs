using System.ComponentModel.DataAnnotations;
using BookkeeperRest.New.Models;
using BookkeeperRest.New.Filters;
using BookkeeperRest.New.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.New.Controllers;

[ApiController]
[Route("api/denomination/break")]
[PasswordAuth]
public class DenominationController : ControllerBase
{

    private readonly IDenominationService denominationService;

    public DenominationController(IDenominationService denominationService)
    {
        this.denominationService = denominationService;
    }

    [HttpGet]
    public IActionResult GetAllDenomination()
    {
        try
        {
            return Ok(denominationService.GetAllDenominations());
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    [HttpPut]
    public IActionResult UpdateAll([FromBody, Required] IEnumerable<Denomination> denominations)
    {
        try
        {
            denominationService.UpdateAllDenominations(denominations);
            return Ok();
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }
}