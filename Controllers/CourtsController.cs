using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BadmintonHub.Services;
using BadmintonHub.ViewModels;

namespace BadmintonHub.Controllers;

public class CourtsController(ICourtService service) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var courts = await service.GetAllAsync(ct);
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