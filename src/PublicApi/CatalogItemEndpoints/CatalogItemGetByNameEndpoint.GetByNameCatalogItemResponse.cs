using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class GetByNameCatalogItemResponse : BaseResponse
{
    public GetByNameCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
    }

    public GetByNameCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
