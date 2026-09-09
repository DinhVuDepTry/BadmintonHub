using Microsoft.AspNetCore.Mvc;
using BadmintonHub.Services;

namespace BadmintonHub.Controllers;

public class CourtsController(ICourtService service) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var courts = await service.GetAllAsync(ct);
        return View(courts);
    }
}