using LibraryManagementSystem.Server.Models;

namespace LibraryManagementSystem.Server.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync(string? search);
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetByIdWithActiveLoansAsync(int id);
    Task<bool> ExistsByIsbnAsync(string isbn, int? excludeId = null);
    Task AddAsync(Book book);
    void Remove(Book book);
    Task<int> GetTotalCopiesAsync();
    Task SaveChangesAsync();
}
