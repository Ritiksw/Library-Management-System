using LibraryManagementSystem.Server.DTOs;

namespace LibraryManagementSystem.Server.Services;

public interface ILoanService
{
    Task<List<LoanDto>> GetAllAsync(bool? activeOnly, string? search);
    Task<ServiceResult<LoanDto>> CheckoutAsync(CreateLoanDto dto);
    Task<ServiceResult> ReturnAsync(int id);
    Task<DashboardDto> GetDashboardAsync();
}
