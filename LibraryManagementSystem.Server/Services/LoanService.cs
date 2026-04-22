using LibraryManagementSystem.Server.DTOs;
using LibraryManagementSystem.Server.Models;
using LibraryManagementSystem.Server.Repositories;

namespace LibraryManagementSystem.Server.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepo;
    private readonly IBookRepository _bookRepo;
    private readonly IMemberRepository _memberRepo;

    public LoanService(ILoanRepository loanRepo, IBookRepository bookRepo, IMemberRepository memberRepo)
    {
        _loanRepo = loanRepo;
        _bookRepo = bookRepo;
        _memberRepo = memberRepo;
    }

    public async Task<List<LoanDto>> GetAllAsync(bool? activeOnly)
    {
        var loans = await _loanRepo.GetAllAsync(activeOnly);
        return loans.Select(MapToDto).ToList();
    }

    public async Task<ServiceResult<LoanDto>> CheckoutAsync(CreateLoanDto dto)
    {
        var book = await _bookRepo.GetByIdAsync(dto.BookId);
        if (book is null)
            return ServiceResult<LoanDto>.NotFound("Book not found.");

        var member = await _memberRepo.GetByIdAsync(dto.MemberId);
        if (member is null)
            return ServiceResult<LoanDto>.NotFound("Member not found.");

        if (!member.IsActive)
            return ServiceResult<LoanDto>.BadRequest("Member account is not active.");

        if (book.AvailableCopies <= 0)
            return ServiceResult<LoanDto>.BadRequest("No copies available for this book.");

        if (await _loanRepo.HasActiveLoanAsync(dto.BookId, dto.MemberId))
            return ServiceResult<LoanDto>.BadRequest("This member already has an active loan for this book.");

        var loan = new Loan
        {
            BookId = dto.BookId,
            MemberId = dto.MemberId,
            BorrowDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(dto.LoanDays)
        };

        book.AvailableCopies--;
        await _loanRepo.AddAsync(loan);
        await _loanRepo.SaveChangesAsync();

        return ServiceResult<LoanDto>.Ok(new LoanDto
        {
            Id = loan.Id,
            BookId = book.Id,
            BookTitle = book.Title,
            MemberId = member.Id,
            MemberName = member.FullName,
            BorrowDate = loan.BorrowDate,
            DueDate = loan.DueDate
        });
    }

    public async Task<ServiceResult> ReturnAsync(int id)
    {
        var loan = await _loanRepo.GetByIdWithBookAsync(id);
        if (loan is null)
            return ServiceResult.NotFound("Loan not found.");

        if (loan.ReturnDate.HasValue)
            return ServiceResult.BadRequest("This loan has already been returned.");

        loan.ReturnDate = DateTime.UtcNow;
        loan.Book.AvailableCopies++;
        await _loanRepo.SaveChangesAsync();

        return ServiceResult.Ok();
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var now = DateTime.UtcNow;
        var recentLoans = await _loanRepo.GetRecentAsync(5);

        return new DashboardDto
        {
            TotalBooks = await _bookRepo.GetTotalCopiesAsync(),
            TotalMembers = await _memberRepo.CountActiveAsync(),
            ActiveLoans = await _loanRepo.CountActiveAsync(),
            OverdueLoans = await _loanRepo.CountOverdueAsync(now),
            RecentLoans = recentLoans.Select(MapToDto).ToList()
        };
    }

    private static LoanDto MapToDto(Loan loan) => new()
    {
        Id = loan.Id,
        BookId = loan.BookId,
        BookTitle = loan.Book.Title,
        MemberId = loan.MemberId,
        MemberName = loan.Member.FullName,
        BorrowDate = loan.BorrowDate,
        DueDate = loan.DueDate,
        ReturnDate = loan.ReturnDate,
        IsReturned = loan.ReturnDate.HasValue,
        IsOverdue = !loan.ReturnDate.HasValue && loan.DueDate < DateTime.UtcNow
    };
}
