using CloudBank.Application.DTOs;
using CloudBank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudBank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(
        ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResponse>> CreateTransaction(
        CreateTransactionRequest request)
    {
        var transaction =
            await _transactionService.CreateTransactionAsync(request);

        return CreatedAtAction(
            nameof(GetTransactionById),
            new { id = transaction.Id },
            transaction);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionResponse>>
        GetTransactionById(Guid id)
    {
        var transaction =
            await _transactionService.GetTransactionByIdAsync(id);

        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpGet("account/{accountId:guid}")]
    public async Task<ActionResult<IEnumerable<TransactionResponse>>>
        GetTransactionsByAccountId(Guid accountId)
    {
        var transactions =
            await _transactionService
                .GetTransactionsByAccountIdAsync(accountId);

        return Ok(transactions);
    }
}