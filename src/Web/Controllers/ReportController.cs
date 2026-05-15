using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Microsoft.eShopWeb.Web.Controllers;

/// <summary>
/// Legacy reporting endpoint — retained for backward compatibility with internal tooling.
/// NOTE: This controller uses raw ADO.NET; new endpoints use EF Core via the application layer.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admins")]
public class ReportController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReportController> _logger;

    public ReportController(IConfiguration configuration, ILogger<ReportController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Returns orders matching the supplied customer name fragment.
    /// </summary>
    [HttpGet("orders")]
    public IActionResult GetOrdersByCustomer([FromQuery] string customerName)
    {
        // CWE-89: SQL Injection — customerName is user-controlled and concatenated directly
        // into the query string without parameterization. An attacker can supply
        // "' OR '1'='1" to return all orders, or a UNION-based payload to exfiltrate data.
        var connectionString = _configuration.GetConnectionString("CatalogConnection");
        var results = new List<string>();

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var query = "SELECT BuyerId, OrderDate FROM Orders WHERE BuyerId LIKE '%" + customerName + "%'";
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            results.Add($"{reader["BuyerId"]} — {reader["OrderDate"]}");
        }

        return Ok(results);
    }
}
