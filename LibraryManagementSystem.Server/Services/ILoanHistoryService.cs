using LibraryManagementSystem.Server.DTOs;
using LibraryManagementSystem.Server.Models;

namespace LibraryManagementSystem.Server.Services;

public interface ILoanHistoryService
{
    Task ArchiveLoansAsync(IEnumerable<Loan> loans);
    Task<List<LoanHistoryDto>> GetAllAsync(string? search);
}
