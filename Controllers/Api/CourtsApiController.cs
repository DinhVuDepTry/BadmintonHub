using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BadmintonHub.Services;
using BadmintonHub.ViewModels;

namespace BadmintonHub.Controllers.Api;

[ApiController]
[Route("api/courts")]
public class CourtsApiController(ICourtService service) : ControllerBase
{
    // Ai cũng xem được danh sách sân (kể cả khách chưa login, nếu muốn public thì bỏ [Authorize])
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourtResponseDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<CourtResponseDto>> GetById(long id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // Chỉ Admin được tạo/sửa/xóa sân -> đây là chỗ thể hiện RBAC rõ nhất
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CourtResponseDto>> Create(CourtCreateDto dto, CancellationToken ct)
    {
        var item = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = item.CourtId }, item);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CourtResponseDto>> Update(long id, CourtUpdateDto dto, CancellationToken ct)
    {
        var item = await service.UpdateAsync(id, dto, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct) =>
        await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}