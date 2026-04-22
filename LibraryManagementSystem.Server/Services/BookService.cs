using LibraryManagementSystem.Server.DTOs;
using LibraryManagementSystem.Server.Models;
using LibraryManagementSystem.Server.Repositories;

namespace LibraryManagementSystem.Server.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepo;
    private readonly ILoanRepository _loanRepo;
    private readonly ILoanHistoryService _historyService;

    public BookService(IBookRepository bookRepo, ILoanRepository loanRepo, ILoanHistoryService historyService)
    {
        _bookRepo = bookRepo;
        _loanRepo = loanRepo;
        _historyService = historyService;
    }

    public async Task<List<BookDto>> GetAllAsync(string? search)
    {
        var books = await _bookRepo.GetAllAsync(search);
        return books.Select(MapToDto).ToList();
    }

    public async Task<ServiceResult<BookDto>> GetByIdAsync(int id)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        if (book is null)
            return ServiceResult<BookDto>.NotFound("Book not found.");

        return ServiceResult<BookDto>.Ok(MapToDto(book));
    }

    public async Task<ServiceResult<BookDto>> CreateAsync(CreateBookDto dto)
    {
        if (await _bookRepo.ExistsByIsbnAsync(dto.ISBN))
            return ServiceResult<BookDto>.Conflict("A book with this ISBN already exists.");

        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            Genre = dto.Genre,
            PublishedYear = dto.PublishedYear,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.TotalCopies
        };

        await _bookRepo.AddAsync(book);
        await _bookRepo.SaveChangesAsync();

        return ServiceResult<BookDto>.Ok(MapToDto(book));
    }

    public async Task<ServiceResult> UpdateAsync(int id, CreateBookDto dto)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        if (book is null)
            return ServiceResult.NotFound("Book not found.");

        if (await _bookRepo.ExistsByIsbnAsync(dto.ISBN, excludeId: id))
            return ServiceResult.Conflict("A book with this ISBN already exists.");

        var loanedCopies = book.TotalCopies - book.AvailableCopies;
        if (dto.TotalCopies < loanedCopies)
            return ServiceResult.BadRequest($"Cannot reduce copies below {loanedCopies} (currently loaned out).");

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.ISBN = dto.ISBN;
        book.Genre = dto.Genre;
        book.PublishedYear = dto.PublishedYear;
        book.AvailableCopies += dto.TotalCopies - book.TotalCopies;
        book.TotalCopies = dto.TotalCopies;

        await _bookRepo.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var book = await _bookRepo.GetByIdWithAllLoansAsync(id);
        if (book is null)
            return ServiceResult.NotFound("Book not found.");

        if (book.Loans.Any(l => l.ReturnDate == null))
            return ServiceResult.BadRequest("Cannot delete a book with active loans. Return all copies first.");

        if (book.Loans.Any())
        {
            await _historyService.ArchiveLoansAsync(book.Loans);
            _loanRepo.RemoveRange(book.Loans);
        }

        _bookRepo.Remove(book);
        await _bookRepo.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    private static BookDto MapToDto(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        ISBN = book.ISBN,
        Genre = book.Genre,
        PublishedYear = book.PublishedYear,
        TotalCopies = book.TotalCopies,
        AvailableCopies = book.AvailableCopies
    };
}
