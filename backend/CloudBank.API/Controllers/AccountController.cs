using CloudBank.Application.DTOs;
using CloudBank.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudBank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(
        [FromBody] CreateAccountRequest request)
    {
        var account = await _accountService.CreateAccountAsync(request);

        return CreatedAtAction(
            nameof(GetAccountById),
            new { id = account.Id },
            account);
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        var accounts = await _accountService.GetAccountsAsync();

        return Ok(accounts);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);

        if (account == null)
        {
            return NotFound();
        }

        return Ok(account);
    }
}