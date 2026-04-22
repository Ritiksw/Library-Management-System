using LibraryManagementSystem.Server.Models;

namespace LibraryManagementSystem.Server.Repositories;

public interface ILoanHistoryRepository
{
    Task<List<LoanHistory>> GetAllAsync(string? search);
    Task AddRangeAsync(IEnumerable<LoanHistory> records);
    Task SaveChangesAsync();
}
