using LibraryManagementSystem.Server.Models;

namespace LibraryManagementSystem.Server.Repositories;

public interface IMemberRepository
{
    Task<List<Member>> GetAllAsync(string? search);
    Task<Member?> GetByIdAsync(int id);
    Task<Member?> GetByIdWithLoansAsync(int id);
    Task<Member?> GetByIdWithActiveLoansAsync(int id);
    Task<Member?> GetByIdWithAllLoansAsync(int id);
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
    Task AddAsync(Member member);
    void Remove(Member member);
    Task<int> CountActiveAsync();
    Task SaveChangesAsync();
}
