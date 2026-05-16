using System;

namespace Microsoft.eShopWeb.PublicApi.BillingEndpoints;

public class CalculateOrderTotalResponse : BaseResponse
{
    public CalculateOrderTotalResponse(Guid correlationId) : base(correlationId) { }

    public decimal OrderTotal { get; set; }
}
