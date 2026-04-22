using LibraryManagementSystem.Server.DTOs;

namespace LibraryManagementSystem.Server.Services;

public interface IMemberService
{
    Task<List<MemberDto>> GetAllAsync(string? search);
    Task<ServiceResult<MemberDto>> GetByIdAsync(int id);
    Task<ServiceResult<MemberDto>> CreateAsync(CreateMemberDto dto);
    Task<ServiceResult> UpdateAsync(int id, CreateMemberDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
