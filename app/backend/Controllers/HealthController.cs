using Microsoft.AspNetCore.Mvc;
using app.backend.Services;
using app.backend.Models;
using app.backend.Patterns.FMethod;

namespace app.backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
	[HttpGet]
	public IActionResult Get() => Ok(new { ok = true, time = DateTime.UtcNow });
}