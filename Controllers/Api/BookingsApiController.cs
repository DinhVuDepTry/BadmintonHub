using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BadmintonHub.Models;
using BadmintonHub.Services;
using BadmintonHub.ViewModels;

namespace BadmintonHub.Controllers.Api;

[ApiController]
[Route("api/bookings")]
[Authorize] // bắt buộc đăng nhập cho mọi endpoint trong controller này
public class BookingsApiController(
    IBookingService bookingService,
    UserManager<ApplicationUser> userManager) : ControllerBase
{
    // GET api/bookings -> Admin xem tất cả, Customer chỉ xem của mình
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookingResponseDto>>> GetAll(CancellationToken ct)
    {
        var isAdmin = User.IsInRole("Admin");
        var userId = userManager.GetUserId(User);

        var result = await bookingService.GetAllAsync(isAdmin ? null : userId, ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<BookingResponseDto>> GetById(long id, CancellationToken ct)
    {
        var item = await bookingService.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST api/bookings -> chỉ Customer đặt sân cho chính mình
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<BookingResponseDto>> Create(
        BookingCreateDto dto, CancellationToken ct)
    {
        var userId = userManager.GetUserId(User)!;
        var result = await bookingService.CreateAsync(userId, dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.BookingId }, result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Cancel(long id, CancellationToken ct)
    {
        var userId = userManager.GetUserId(User)!;
        var success = await bookingService.CancelAsync(id, userId, ct);
        return success ? NoContent() : NotFound();
    }
}