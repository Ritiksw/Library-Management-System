using LibraryManagementSystem.Server.Models;

namespace LibraryManagementSystem.Server.Repositories;

public interface ILoanRepository
{
    Task<List<Loan>> GetAllAsync(bool? activeOnly, string? search);
    Task<Loan?> GetByIdWithBookAsync(int id);
    Task<bool> HasActiveLoanAsync(int bookId, int memberId);
    Task AddAsync(Loan loan);
    void RemoveRange(IEnumerable<Loan> loans);
    Task<int> CountActiveAsync();
    Task<int> CountOverdueAsync(DateTime now);
    Task<List<Loan>> GetRecentAsync(int count);
    Task SaveChangesAsync();
}
