using LibraryManagementSystem.Server.DTOs;
using LibraryManagementSystem.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Server.Controllers;

[Route("api/[controller]")]
public class MembersController : ApiControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService) => _memberService = memberService;

    [HttpGet]
    public async Task<ActionResult<List<MemberDto>>> GetAll([FromQuery] string? search) =>
        Ok(await _memberService.GetAllAsync(search));

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetById(int id) =>
        ToActionResult(await _memberService.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<MemberDto>> Create(CreateMemberDto dto)
    {
        var result = await _memberService.CreateAsync(dto);
        if (!result.Success) return ToActionResult(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateMemberDto dto) =>
        ToActionResult(await _memberService.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) =>
        ToActionResult(await _memberService.DeleteAsync(id));
}
