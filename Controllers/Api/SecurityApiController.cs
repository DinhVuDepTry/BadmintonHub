using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonHub.Controllers.Api;

[ApiController]
[Route("api/security")]
public class SecurityApiController(IAntiforgery antiforgery) : ControllerBase
{
    [HttpGet("csrf")]
    [Authorize]
    public IActionResult GetCsrfToken()
    {
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);
        return Ok(new { token = tokens.RequestToken });
    }
}