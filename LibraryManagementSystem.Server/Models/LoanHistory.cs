using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Server.Models;

public class LoanHistory
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string BookTitle { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string MemberName { get; set; } = string.Empty;

    public DateTime BorrowDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public DateTime ArchivedAt { get; set; } = DateTime.UtcNow;
}
