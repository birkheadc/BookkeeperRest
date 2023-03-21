using System.ComponentModel.DataAnnotations;
using BookkeeperRest.New.Filters;
using BookkeeperRest.New.Models;
using BookkeeperRest.New.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.New.Controllers;

[ApiController]
[Route("api/report")]
[PasswordAuth]
public class ReportController : ControllerBase
{
    private readonly IReportService reportService;
    private readonly IWebHostEnvironment env;

    public ReportController(IReportService reportService, IWebHostEnvironment env)
    {
        this.reportService = reportService;
        this.env = env;
    }

    [HttpGet]
    public IActionResult Get([FromQuery(Name = "startDate"), Required] DateTime startDate, [FromQuery(Name = "endDate"), Required] DateTime endDate)
    {
        if (endDate < startDate) {
            return BadRequest("End Date must be the same as or after the Start Date!");
        }
        try
        {
            ReportsWrapper reports = reportService.GenerateReportForDatesBetween(startDate, endDate);
            return Ok(reports);
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    [HttpGet]
    [Route("breakdowns")]
    public IActionResult GetBreakdowns([FromQuery(Name = "startDate"), Required] DateTime startDate, [FromQuery(Name = "endDate"), Required] DateTime endDate)
    {
        if (endDate < startDate) {
            return BadRequest("End Date must be the same as or after the Start Date!");
        }
        try
        {
            SpecialBreakdowns breakdowns = reportService.GenerateSpecialBreakdownsForDatesBetween(startDate, endDate);
            return Ok(breakdowns);
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    [HttpPut]
    public IActionResult AddOrUpdateReport([FromBody, Required] ReportDtoIncoming reportDtoIncoming)
    {
        try
        {
            reportService.ProcessReport(reportDtoIncoming);
            return Ok();
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    [HttpPost]
    [Route("csv")]
    public IActionResult ProcessCsv(IFormFile file)
    {
        try
        {
            reportService.ProcessCsv(file);
            return Ok();
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    [HttpPost]
    [Route("mass-report")]
    public IActionResult MassReport([FromBody] MassReport report)
    {
      try
      {
        reportService.ProcessMassReport(report);
        return Ok();
      }
      catch
      {
        return BadRequest("Something went wrong...");
      }
    }

    [HttpPost]
    [Route("debug/populate")]
    public IActionResult PopulateWithTestData()
    {
        if (env.IsEnvironment("Development") == false)
        {
            return Unauthorized();
        }

        try
        {
            List<ReportDtoIncoming> reports = GenerateWeekOfRandomReports();

            foreach (ReportDtoIncoming report in reports)
            {
                reportService.ProcessReport(report);
            }

            return Ok();
        }
        catch
        {
            return BadRequest("Failed to populate the database...");
        }
    }

    private static List<ReportDtoIncoming> GenerateWeekOfRandomReports()
    {
        List<ReportDtoIncoming> reports = new();
        Random random = new();

        for (int i = 0; i < 7; i++)
        {
            DateTime date = DateTime.Now.AddDays(i * -1);
            List<EarningDtoIncoming> earnings = new();

            earnings.Add(new EarningDtoIncoming()
            {
                Category = "cash",
                Amount = random.Next(1000, 10000),
                Date = date
            });

            earnings.Add(new EarningDtoIncoming()
            {
                Category = "card",
                Amount = random.Next(300, 3000),
                Date = date
            });

            earnings.Add(new EarningDtoIncoming()
            {
                Category = "coupon",
                Amount = random.Next(100, 1000),
                Date = date
            });

            List<ExpenseDtoIncoming> expenses = new();

            expenses.Add(new ExpenseDtoIncoming()
            {
                Category = "delivery",
                Amount = random.Next(6, 60),
                Date = date,
                Note = "delivery, paid for out of the register",
                WasTakenFromCash = true
            });

            expenses.Add(new ExpenseDtoIncoming()
            {
                Category = "Lunch break",
                Amount = random.Next(10, 100),
                Date = date,
                Note = ""
            });

            ReportDtoIncoming report = new()
            {
                Date = date,
                Earnings = earnings,
                Expenses = expenses
            };
            reports.Add(report);
        }

        return reports;
    }
}