using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BadmintonHub.Models;
using BadmintonHub.Services;
using BadmintonHub.ViewModels;

namespace BadmintonHub.Controllers;

[Authorize(Roles = "Admin,Customer")]
public class BookingsController(
    IBookingService bookingService,
    ICourtService courtService,
    UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var isAdmin = User.IsInRole("Admin");
        var userId = userManager.GetUserId(User);
        var bookings = await bookingService.GetAllAsync(isAdmin ? null : userId, ct);
        return View(bookings);
    }

    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Create(long? courtId, CancellationToken ct)
    {
        ViewBag.Courts = (await courtService.GetAllAsync(ct))
            .Where(c => c.Status == Models.Enums.CourtStatus.Active)
            .ToList();
        ViewBag.SelectedCourtId = courtId;
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Customer")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Cancel(long id, CancellationToken ct)
{
    var userId = userManager.GetUserId(User)!;
    await bookingService.CancelAsync(id, userId, User.IsInRole("Admin"), ct);
    return RedirectToAction(nameof(Index));
}


    [HttpPost]
    [Authorize(Roles = "Customer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingCreateDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Courts = (await courtService.GetAllAsync(ct))
                .Where(c => c.Status == Models.Enums.CourtStatus.Active)
                .ToList();
            return View(dto);
        }

        var userId = userManager.GetUserId(User)!;
        try
        {
            await bookingService.CreateAsync(userId, dto, ct);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.Courts = (await courtService.GetAllAsync(ct))
                .Where(c => c.Status == Models.Enums.CourtStatus.Active)
                .ToList();
            return View(dto);
        }
    }
}