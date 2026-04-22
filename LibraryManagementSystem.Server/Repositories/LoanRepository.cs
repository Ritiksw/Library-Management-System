using LibraryManagementSystem.Server.Data;
using LibraryManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Server.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly LibraryDbContext _db;

    public LoanRepository(LibraryDbContext db) => _db = db;

    public async Task<List<Loan>> GetAllAsync(bool? activeOnly, string? search)
    {
        var query = _db.Loans.Include(l => l.Book).Include(l => l.Member).AsQueryable();

        if (activeOnly == true)
            query = query.Where(l => l.ReturnDate == null);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(l =>
                l.Book.Title.ToLower().Contains(term) ||
                l.Member.FullName.ToLower().Contains(term));
        }

        return await query.OrderByDescending(l => l.BorrowDate).ToListAsync();
    }

    public async Task<Loan?> GetByIdWithBookAsync(int id) =>
        await _db.Loans.Include(l => l.Book).Include(l => l.Member).FirstOrDefaultAsync(l => l.Id == id);

    public async Task<bool> HasActiveLoanAsync(int bookId, int memberId) =>
        await _db.Loans.AnyAsync(l => l.BookId == bookId && l.MemberId == memberId && l.ReturnDate == null);

    public async Task AddAsync(Loan loan) => await _db.Loans.AddAsync(loan);

    public void RemoveRange(IEnumerable<Loan> loans) => _db.Loans.RemoveRange(loans);

    public async Task<int> CountActiveAsync() =>
        await _db.Loans.CountAsync(l => l.ReturnDate == null);

    public async Task<int> CountOverdueAsync(DateTime now) =>
        await _db.Loans.CountAsync(l => l.ReturnDate == null && l.DueDate < now);

    public async Task<List<Loan>> GetRecentAsync(int count) =>
        await _db.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .OrderByDescending(l => l.BorrowDate)
            .Take(count)
            .ToListAsync();

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
