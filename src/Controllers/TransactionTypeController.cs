using BookkeeperRest.Filters;
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
}