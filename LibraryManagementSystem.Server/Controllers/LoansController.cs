using LibraryManagementSystem.Server.DTOs;
using LibraryManagementSystem.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Server.Controllers;

[Route("api/[controller]")]
public class LoansController : ApiControllerBase
{
    private readonly ILoanService _loanService;
    private readonly ILoanHistoryService _historyService;

    public LoansController(ILoanService loanService, ILoanHistoryService historyService)
    {
        _loanService = loanService;
        _historyService = historyService;
    }

    [HttpGet]
    public async Task<ActionResult<List<LoanDto>>> GetAll([FromQuery] bool? activeOnly, [FromQuery] string? search) =>
        Ok(await _loanService.GetAllAsync(activeOnly, search));

    [HttpPost]
    public async Task<ActionResult<LoanDto>> Checkout(CreateLoanDto dto) =>
        ToActionResult(await _loanService.CheckoutAsync(dto));

    [HttpPost("{id}/return")]
    public async Task<ActionResult> Return(int id)
    {
        var result = await _loanService.ReturnAsync(id);
        if (result.Success)
            return Ok(new { message = "Book returned successfully." });
        return ToActionResult(result);
    }

    [HttpGet("history")]
    public async Task<ActionResult<List<LoanHistoryDto>>> History([FromQuery] string? search) =>
        Ok(await _historyService.GetAllAsync(search));

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardDto>> Dashboard() =>
        Ok(await _loanService.GetDashboardAsync());
}
