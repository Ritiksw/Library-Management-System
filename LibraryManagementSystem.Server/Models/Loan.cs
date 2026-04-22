namespace LibraryManagementSystem.Server.Models;

public class Loan
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public bool IsReturned => ReturnDate.HasValue;

    public bool IsOverdue => !IsReturned && DueDate < DateTime.UtcNow;
}
