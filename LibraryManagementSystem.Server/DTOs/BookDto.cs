using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Server.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}

public class CreateBookDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Author { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Genre { get; set; } = string.Empty;

    public int PublishedYear { get; set; }

    [Range(1, 100)]
    public int TotalCopies { get; set; } = 1;
}
