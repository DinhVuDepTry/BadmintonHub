using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BadmintonHub.Data;
using BadmintonHub.Models.Enums;
using BadmintonHub.ViewModels;

namespace BadmintonHub.Controllers;

[Authorize(Roles = "Admin")]
public sealed class AdminController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var bookings = db.Bookings.AsNoTracking().Include(b => b.Court);
        var upcoming = await bookings
            .Where(b => b.BookingDate >= today && b.Status != BookingStatus.Cancelled)
            .OrderBy(b => b.BookingDate)
            .ThenBy(b => b.StartTime)
            .Take(6)
            .Select(b => new BookingResponseDto(
                b.BookingId, b.CourtId, b.Court.CourtName, b.CustomerId,
                b.BookingDate, b.StartTime, b.EndTime,
                b.Status.ToString(), b.TotalPrice))
            .ToListAsync(ct);

        var todayBookings = await bookings
            .Where(b => b.BookingDate == today && b.Status != BookingStatus.Cancelled)
            .ToListAsync(ct);

        var model = new AdminDashboardViewModel(
            await db.Courts.CountAsync(c => c.Status == CourtStatus.Active, ct),
            await db.Bookings.CountAsync(b => b.Status == BookingStatus.Pending, ct),
            todayBookings.Count,
            todayBookings.Sum(b => b.TotalPrice),
            upcoming);

        return View(model);
    }
}
