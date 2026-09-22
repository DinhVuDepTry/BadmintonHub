using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BadmintonHub.Services;
using BadmintonHub.ViewModels;

namespace BadmintonHub.Controllers;

public class CourtsController(ICourtService service) : Controller
{
    public async Task<IActionResult> Index(
        DateOnly? date, TimeOnly? startTime, TimeOnly? endTime, CancellationToken ct)
    {
        var hasSearch = date.HasValue || startTime.HasValue || endTime.HasValue;
        var courts = hasSearch
            ? await service.GetAvailableAsync(date, startTime, endTime, ct)
            : await service.GetAllAsync(ct);

        if (!User.IsInRole("Admin"))
            courts = courts.Where(c => c.Status == Models.Enums.CourtStatus.Active).ToList();

        ViewBag.SearchDate = date?.ToString("yyyy-MM-dd");
        ViewBag.SearchStartTime = startTime?.ToString("HH:mm");
        ViewBag.SearchEndTime = endTime?.ToString("HH:mm");
        ViewBag.HasSearch = hasSearch;
        return View(courts);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourtCreateDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(dto);
        try
        {
            await service.CreateAsync(dto, ct);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        try { await service.DeleteAsync(id, ct); }
        catch (Exception ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }
}