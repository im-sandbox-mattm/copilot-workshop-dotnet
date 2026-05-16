using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Services;
using MinimalApi.Endpoint;

namespace Microsoft.eShopWeb.PublicApi.BillingEndpoints;

/// <summary>
/// Calculates the order total for a given customer and basket.
/// WORKSHOP ARTIFACT — wires BillingCalculationService (used in Labs 7, 10, 12) into the running API.
/// </summary>
public class CalculateOrderTotalEndpoint : IEndpoint<IResult, CalculateOrderTotalRequest>
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("api/billing/calculate",
            async (CalculateOrderTotalRequest request) =>
            {
                return await HandleAsync(request);
            })
            .Produces<CalculateOrderTotalResponse>()
            .WithTags("BillingEndpoints");
    }

    public Task<IResult> HandleAsync(CalculateOrderTotalRequest request)
    {
        var response = new CalculateOrderTotalResponse(request.CorrelationId());

        var service = new BillingCalculationService();
        response.OrderTotal = service.CalculateOrderTotal(
            request.CustomerId,
            request.MembershipTier,
            request.CatalogSubtotal,
            request.IsPeakSeason,
            request.LoyaltyYears,
            request.PromoCode);

        return Task.FromResult(Results.Ok(response));
    }
}
