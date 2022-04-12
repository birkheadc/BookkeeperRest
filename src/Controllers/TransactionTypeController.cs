using System.Transactions;
using BookkeeperRest.Filters;
using BookkeeperRest.Models;
using BookkeeperRest.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.Controllers;

[ApiController]
[Route("transactiontype")]
[PasswordAuth]
public class TransactionTypeController : ControllerBase
{
    private ITransactionTypeService service;

    public TransactionTypeController(ITransactionTypeService service)
    {
        this.service = service;
    }

    [HttpPost]
    public IActionResult AddNew([FromBody] TransactionType type)
    {
        try
        {
            service.Add(type);
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
    [Route("{name}")]
    public IActionResult DeleteByName(string name)
    {
        try
        {
            service.RemoveByName(name);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}