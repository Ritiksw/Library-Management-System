using LibraryManagementSystem.Server.DTOs;
using LibraryManagementSystem.Server.Models;
using LibraryManagementSystem.Server.Repositories;

namespace LibraryManagementSystem.Server.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepo;

    public MemberService(IMemberRepository memberRepo) => _memberRepo = memberRepo;

    public async Task<List<MemberDto>> GetAllAsync(string? search)
    {
        var members = await _memberRepo.GetAllAsync(search);
        return members.Select(MapToDto).ToList();
    }

    public async Task<ServiceResult<MemberDto>> GetByIdAsync(int id)
    {
        var member = await _memberRepo.GetByIdWithLoansAsync(id);
        if (member is null)
            return ServiceResult<MemberDto>.NotFound("Member not found.");

        return ServiceResult<MemberDto>.Ok(MapToDto(member));
    }

    public async Task<ServiceResult<MemberDto>> CreateAsync(CreateMemberDto dto)
    {
        if (await _memberRepo.ExistsByEmailAsync(dto.Email))
            return ServiceResult<MemberDto>.Conflict("A member with this email already exists.");

        var member = new Member
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone
        };

        await _memberRepo.AddAsync(member);
        await _memberRepo.SaveChangesAsync();

        return ServiceResult<MemberDto>.Ok(MapToDto(member));
    }

    public async Task<ServiceResult> UpdateAsync(int id, CreateMemberDto dto)
    {
        var member = await _memberRepo.GetByIdAsync(id);
        if (member is null)
            return ServiceResult.NotFound("Member not found.");

        if (await _memberRepo.ExistsByEmailAsync(dto.Email, excludeId: id))
            return ServiceResult.Conflict("A member with this email already exists.");

        member.FullName = dto.FullName;
        member.Email = dto.Email;
        member.Phone = dto.Phone;

        await _memberRepo.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var member = await _memberRepo.GetByIdWithActiveLoansAsync(id);
        if (member is null)
            return ServiceResult.NotFound("Member not found.");

        if (member.Loans.Any())
            return ServiceResult.BadRequest("Cannot delete a member with active loans.");

        _memberRepo.Remove(member);
        await _memberRepo.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    private static MemberDto MapToDto(Member member) => new()
    {
        Id = member.Id,
        FullName = member.FullName,
        Email = member.Email,
        Phone = member.Phone,
        MembershipDate = member.MembershipDate,
        IsActive = member.IsActive,
        ActiveLoans = member.Loans.Count(l => l.ReturnDate == null)
    };
}
