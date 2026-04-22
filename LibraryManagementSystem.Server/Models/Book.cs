using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Server.Models;

public class Book
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Author { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Genre { get; set; } = string.Empty;

    public int PublishedYear { get; set; }

    public int TotalCopies { get; set; } = 1;

    public int AvailableCopies { get; set; } = 1;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
