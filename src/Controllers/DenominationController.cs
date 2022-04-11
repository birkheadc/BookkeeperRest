using BookkeeperRest.Filters;
using BookkeeperRest.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.Controllers;

[ApiController]
[Route("denomination")]
[PasswordAuth]
public class DenominationController : ControllerBase
{
    private IDenominationService denominationService;

    public DenominationController(IDenominationService denominationService)
    {
        this.denominationService = denominationService;
    }
}