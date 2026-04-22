using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Server.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime MembershipDate { get; set; }
    public bool IsActive { get; set; }
    public int ActiveLoans { get; set; }
}

public class CreateMemberDto
{
    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(200), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
}
