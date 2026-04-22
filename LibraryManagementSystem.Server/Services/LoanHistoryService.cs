using LibraryManagementSystem.Server.DTOs;
using LibraryManagementSystem.Server.Models;
using LibraryManagementSystem.Server.Repositories;

namespace LibraryManagementSystem.Server.Services;

public class LoanHistoryService : ILoanHistoryService
{
    private readonly ILoanHistoryRepository _historyRepo;

    public LoanHistoryService(ILoanHistoryRepository historyRepo) => _historyRepo = historyRepo;

    public async Task ArchiveLoansAsync(IEnumerable<Loan> loans)
    {
        var records = loans.Select(l => new LoanHistory
        {
            BookTitle = l.Book.Title,
            MemberName = l.Member.FullName,
            BorrowDate = l.BorrowDate,
            DueDate = l.DueDate,
            ReturnDate = l.ReturnDate,
            ArchivedAt = DateTime.UtcNow
        });

        await _historyRepo.AddRangeAsync(records);
    }

    public async Task<List<LoanHistoryDto>> GetAllAsync(string? search)
    {
        var records = await _historyRepo.GetAllAsync(search);
        return records.Select(h => new LoanHistoryDto
        {
            Id = h.Id,
            BookTitle = h.BookTitle,
            MemberName = h.MemberName,
            BorrowDate = h.BorrowDate,
            DueDate = h.DueDate,
            ReturnDate = h.ReturnDate,
            ArchivedAt = h.ArchivedAt
        }).ToList();
    }
}
