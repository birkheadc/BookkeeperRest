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
}