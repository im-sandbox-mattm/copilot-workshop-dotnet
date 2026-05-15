using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopWeb.Web.Controllers;

/// <summary>
/// Handles notification delivery for order and account events.
/// NOTE: This controller uses the legacy MVC pattern; new endpoints use Razor Pages.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(ILogger<NotificationController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs a notification request for the given account.
    /// </summary>
    [HttpPost("send")]
    public IActionResult SendNotification([FromBody] NotificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.AccountId) || string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("AccountId and Message are required.");
        }

        // CWE-117: Log Injection — AccountId is user-controlled and embedded via string
        // interpolation rather than a structured logging placeholder. An attacker can inject
        // newline characters into AccountId to forge additional log entries.
        // CORRECT:   _logger.LogInformation("Notification sent for account {AccountId}", request.AccountId);
        // INCORRECT (this code):
        _logger.LogInformation($"Notification sent for account {request.AccountId}: {request.Message}");

        return Ok(new { Status = "queued" });
    }
}

public record NotificationRequest(string AccountId, string Message);
