using LibraryManagementSystem.Server.Data;
using LibraryManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Server.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly LibraryDbContext _db;

    public MemberRepository(LibraryDbContext db) => _db = db;

    public async Task<List<Member>> GetAllAsync(string? search)
    {
        var query = _db.Members.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(m =>
                m.FullName.ToLower().Contains(search) ||
                m.Email.ToLower().Contains(search));
        }

        return await query.OrderBy(m => m.FullName)
            .Include(m => m.Loans)
            .ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(int id) =>
        await _db.Members.FindAsync(id);

    public async Task<Member?> GetByIdWithLoansAsync(int id) =>
        await _db.Members.Include(m => m.Loans).FirstOrDefaultAsync(m => m.Id == id);

    public async Task<Member?> GetByIdWithActiveLoansAsync(int id) =>
        await _db.Members
            .Include(m => m.Loans.Where(l => l.ReturnDate == null))
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task<Member?> GetByIdWithAllLoansAsync(int id) =>
        await _db.Members
            .Include(m => m.Loans).ThenInclude(l => l.Book)
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null) =>
        await _db.Members.AnyAsync(m => m.Email == email && (!excludeId.HasValue || m.Id != excludeId.Value));

    public async Task AddAsync(Member member) => await _db.Members.AddAsync(member);

    public void Remove(Member member) => _db.Members.Remove(member);

    public async Task<int> CountActiveAsync() =>
        await _db.Members.CountAsync(m => m.IsActive);

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
