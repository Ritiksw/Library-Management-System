using LibraryManagementSystem.Server.Models;

namespace LibraryManagementSystem.Server.Repositories;

public interface ILoanRepository
{
    Task<List<Loan>> GetAllAsync(bool? activeOnly);
    Task<Loan?> GetByIdWithBookAsync(int id);
    Task<bool> HasActiveLoanAsync(int bookId, int memberId);
    Task AddAsync(Loan loan);
    Task<int> CountActiveAsync();
    Task<int> CountOverdueAsync(DateTime now);
    Task<List<Loan>> GetRecentAsync(int count);
    Task SaveChangesAsync();
}
