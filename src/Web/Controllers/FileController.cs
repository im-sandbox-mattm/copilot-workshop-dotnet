using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopWeb.Web.Controllers;

/// <summary>
/// Serves files from the application's download directory.
/// NOTE: This controller uses the legacy MVC pattern; new endpoints use Razor Pages.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly string _downloadBasePath = Path.Combine(Directory.GetCurrentDirectory(), "downloads");

    /// <summary>
    /// Downloads a file by name from the server's download directory.
    /// </summary>
    [HttpGet("{fileName}")]
    public IActionResult DownloadFile(string fileName)
    {
        // CWE-22: Path traversal — fileName is user-controlled and not sanitized.
        // An attacker can pass "../../../etc/passwd" or "..\..\..\appsettings.json"
        // to read arbitrary files outside the intended download directory.
        var filePath = Path.Combine(_downloadBasePath, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        var contentType = "application/octet-stream";
        return File(fileBytes, contentType, fileName);
    }
}
