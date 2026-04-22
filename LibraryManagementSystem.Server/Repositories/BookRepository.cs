using LibraryManagementSystem.Server.Data;
using LibraryManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Server.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _db;

    public BookRepository(LibraryDbContext db) => _db = db;

    public async Task<List<Book>> GetAllAsync(string? search)
    {
        var query = _db.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(b =>
                b.Title.ToLower().Contains(search) ||
                b.Author.ToLower().Contains(search) ||
                b.ISBN.Contains(search) ||
                b.Genre.ToLower().Contains(search));
        }

        return await query.OrderBy(b => b.Title).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id) =>
        await _db.Books.FindAsync(id);

    public async Task<Book?> GetByIdWithActiveLoansAsync(int id) =>
        await _db.Books
            .Include(b => b.Loans.Where(l => l.ReturnDate == null))
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<bool> ExistsByIsbnAsync(string isbn, int? excludeId = null) =>
        await _db.Books.AnyAsync(b => b.ISBN == isbn && (!excludeId.HasValue || b.Id != excludeId.Value));

    public async Task AddAsync(Book book) => await _db.Books.AddAsync(book);

    public void Remove(Book book) => _db.Books.Remove(book);

    public async Task<int> GetTotalCopiesAsync() =>
        await _db.Books.SumAsync(b => b.TotalCopies);

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
