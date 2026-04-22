using LibraryManagementSystem.Server.DTOs;
using LibraryManagementSystem.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Server.Controllers;

[Route("api/[controller]")]
public class BooksController : ApiControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService) => _bookService = bookService;

    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAll([FromQuery] string? search) =>
        Ok(await _bookService.GetAllAsync(search));

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(int id) =>
        ToActionResult(await _bookService.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(CreateBookDto dto)
    {
        var result = await _bookService.CreateAsync(dto);
        if (!result.Success) return ToActionResult(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateBookDto dto) =>
        ToActionResult(await _bookService.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) =>
        ToActionResult(await _bookService.DeleteAsync(id));
}
