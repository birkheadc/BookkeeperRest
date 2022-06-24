using System.Transactions;
using BookkeeperRest.Filters;
using BookkeeperRest.Models;
using BookkeeperRest.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.Controllers;

// [ApiController]
// [Route("api/transactiontype")]
// [PasswordAuth]
public class TransactionTypeController : ControllerBase
{
    private ITransactionTypeService service;

    public TransactionTypeController(ITransactionTypeService service)
    {
        this.service = service;
    }

    [HttpPost]
    public IActionResult AddNew([FromBody] TransactionType[] types)
    {
        try
        {
            service.Add(types);
            return Ok();
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
    public IActionResult DeleteMultiple([FromBody] IEnumerable<string> names)
    {
        try
        {
            service.RemoveMultiple(names);
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

    [HttpPut]
    public IActionResult AddOrUpdate([FromBody] TransactionType transactionType)
    {
        try
        {
            service.Update(transactionType);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}