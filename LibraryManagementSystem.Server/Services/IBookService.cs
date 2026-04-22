using LibraryManagementSystem.Server.DTOs;

namespace LibraryManagementSystem.Server.Services;

public interface IBookService
{
    Task<List<BookDto>> GetAllAsync(string? search);
    Task<ServiceResult<BookDto>> GetByIdAsync(int id);
    Task<ServiceResult<BookDto>> CreateAsync(CreateBookDto dto);
    Task<ServiceResult> UpdateAsync(int id, CreateBookDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
