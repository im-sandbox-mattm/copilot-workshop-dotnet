namespace Microsoft.eShopWeb.PublicApi.BillingEndpoints;

public class CalculateOrderTotalRequest : BaseRequest
{
    public string CustomerId { get; set; } = string.Empty;
    public string MembershipTier { get; set; } = string.Empty;
    public decimal CatalogSubtotal { get; set; }
    public bool IsPeakSeason { get; set; }
    public int LoyaltyYears { get; set; }
    public string? PromoCode { get; set; }
}
