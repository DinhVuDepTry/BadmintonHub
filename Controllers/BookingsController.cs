using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BadmintonHub.Models;
using BadmintonHub.Services;
using BadmintonHub.ViewModels;

namespace BadmintonHub.Controllers;

[Authorize]
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

    public async Task<IActionResult> Create(CancellationToken ct)
    {
        ViewBag.Courts = await courtService.GetAllAsync(ct);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingCreateDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Courts = await courtService.GetAllAsync(ct);
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
            ViewBag.Courts = await courtService.GetAllAsync(ct);
            return View(dto);
        }
    }
}