using LibraryManagementSystem.Server.Data;
using LibraryManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Server.Repositories;

public class LoanHistoryRepository : ILoanHistoryRepository
{
    private readonly LibraryDbContext _db;

    public LoanHistoryRepository(LibraryDbContext db) => _db = db;

    public async Task<List<LoanHistory>> GetAllAsync(string? search)
    {
        var query = _db.LoanHistory.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(h =>
                h.BookTitle.ToLower().Contains(term) ||
                h.MemberName.ToLower().Contains(term));
        }

        return await query.OrderByDescending(h => h.ArchivedAt).ToListAsync();
    }

    public async Task AddRangeAsync(IEnumerable<LoanHistory> records) =>
        await _db.LoanHistory.AddRangeAsync(records);

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
