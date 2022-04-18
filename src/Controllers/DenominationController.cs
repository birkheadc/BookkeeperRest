using BookkeeperRest.Filters;
using BookkeeperRest.Models;
using BookkeeperRest.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.Controllers;

[ApiController]
[Route("denomination")]
[PasswordAuth]
public class DenominationController : ControllerBase
{
    private IDenominationService service;

    public DenominationController(IDenominationService denominationService)
    {
        this.service = denominationService;
    }

    [HttpPost]
    public IActionResult AddNew([FromBody] Denomination[] denominations)
    {
        try
        {
            service.Add(denominations);
            return Ok();
        }
        catch(DuplicateEntryException)
        {
            return Conflict();
        }
        catch
        {
            return BadRequest();
        }
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

    [HttpDelete]
    public IActionResult DeleteAll()
    {
        try
        {
            service.RemoveAll();
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    [Route("{value}")]
    public IActionResult DeleteByValue(int value)
    {
        try
        {
            service.RemoveByValue(value);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}