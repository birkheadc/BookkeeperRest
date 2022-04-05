using BookkeeperRest.Models.Ledger;
using BookkeeperRest.Models.Report;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.Controllers;

[ApiController]
[Route("api/ledgers")]
public class LedgerController : ControllerBase
{
    [HttpPost]
    [Route("test")]
    public IActionResult Test()
    {
        return Ok();
    }
}