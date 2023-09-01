using BankingSystem.Core.Interfaces;
using BankingSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Account>> CreateAccount([Required] int userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newAccount = await _accountService.CreateAccount(userId);
        return Ok(newAccount);
    }

    [HttpDelete("{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> DeleteAccount(int accountId)
    {
        var result = await _accountService.DeleteAccount(accountId);
        return Ok(result);
    }

}