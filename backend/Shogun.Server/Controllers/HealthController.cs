using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shogun.Server.DTO;
using Shogun.Server.Services;

namespace Shogun.Server.Controllers;

/// <summary>
/// The health endpoint. The web app calls it to confirm that an address
/// really belongs to a Shogun server before trying anything else.
/// </summary>
[Route("api/v1/health")]
[ApiController]
public class HealthController : ControllerBase
{
    private readonly HealthService _healthService;

    public HealthController(HealthService healthService)
    {
        _healthService = healthService;
    }

    /// <summary>
    /// Returns the server's status, version, name, and ID.
    /// </summary>
    [HttpGet(Name = "GetHealth")]
    [AllowAnonymous]
    public ActionResult<HealthResponse> GetHealth()
    {
        return Ok(_healthService.GetHealth());
    }
}