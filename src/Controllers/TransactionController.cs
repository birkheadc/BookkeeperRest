using System.Text.Json;
using BookkeeperRest.Filters;
using BookkeeperRest.Models.Report;
using BookkeeperRest.Models.Summary;
using BookkeeperRest.Models.Transaction;
using BookkeeperRest.Services.TransactionService;
using BookkeeperREst.Models.Csv;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.Controllers;

[ApiController]
[Route("api/transaction")]
[PasswordAuth]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService service;

    public TransactionController(ITransactionService service)
    {
        this.service = service;
    }

    [HttpGet]
    public IActionResult GetAllTransactionsNewestFirst()
    {
        try
        {
            return Ok(service.GetAllTransactionsNewestFirst());
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("summary")]
    public IActionResult GetSummary([FromQuery(Name = "startDate")] DateTime startDate, [FromQuery(Name = "endDate")] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("Start date must be less than or equal to end date.");
        }
        try
        {
            Summary summary = service.BuildSummary(startDate, endDate);
            return Ok(summary);
        }
        catch
        {
            return BadRequest();
        }
        
    }

    [HttpPost]
    [Route("report")]
    public IActionResult ReportTransactions([FromBody] Report report)
    {
        try
        {
            service.HandleReport(report);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("debug/populate")]
    public IActionResult PopulateWithTestData()
    {
        List<NewTransactionDTO> transactions = new();

        for (int i = 0; i < 5; i++)
        {
            transactions.Add(
                new NewTransactionDTO()
                {
                    Date = DateTime.Now.AddDays(i),
                    Type = "cash",
                    Amount = 1000,
                    Note = "daily cash earnings"
                }
            );

            transactions.Add(
                new NewTransactionDTO()
                {
                    Date = DateTime.Now.AddDays(i),
                    Type = "card",
                    Amount = 600,
                    Note = "daily card earnings"
                }
            );

            transactions.Add(
                new NewTransactionDTO()
                {
                    Date = DateTime.Now.AddDays(i),
                    Type = "delivery",
                    Amount = -25,
                    Note = "delivery fee for goods"
                }
            );

            transactions.Add(
                new NewTransactionDTO()
                {
                    Date = DateTime.Now.AddDays(i),
                    Type = "supplies",
                    Amount = -18,
                    Note = "styrofoam trays and plastic wrap"
                }
            );
        }



        Report report = new()
        {
            Transactions = transactions
        };

        try
        {
            service.HandleReport(report);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("debug/ping")]
    public IActionResult Ping()
    {
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteById([FromRoute] string id)
    {
        try
        {   service.DeleteById(id);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut]
    [Route("report")]
    public IActionResult UpdateMultiple([FromBody] UpdateReport report)
    {
        try
        {
            service.UpdateMultiple(report);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("past-n-days")]
    public IActionResult GetPastNDays([FromQuery(Name = "n")] int n)
    {
        try
        {
            return Ok(service.BuildSummariesForPastNDays(n));
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("csv")]
    public IActionResult UploadCsv([FromForm] CsvDto csv)
    {
        try
        {
            service.RecordCsv(csv);
            return Ok();
        }
        catch
        {
            return NoContent();
        }
        
        // try
        // {
        //     service.RecordCsv(csv.File);
        //     return Ok();
        // }
        // catch
        // {
        //     return NoContent();
        // }
    }
}