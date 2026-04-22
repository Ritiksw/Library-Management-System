using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Server.DTOs;

public class LoanDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned { get; set; }
    public bool IsOverdue { get; set; }
}

public class CreateLoanDto
{
    [Required]
    public int BookId { get; set; }

    [Required]
    public int MemberId { get; set; }

    [Range(1, 90)]
    public int LoanDays { get; set; } = 14;
}

public class LoanHistoryDto
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string MemberName { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime ArchivedAt { get; set; }
}

public class DashboardDto
{
    public int TotalBooks { get; set; }
    public int TotalMembers { get; set; }
    public int ActiveLoans { get; set; }
    public int OverdueLoans { get; set; }
    public List<LoanDto> RecentLoans { get; set; } = new();
}
