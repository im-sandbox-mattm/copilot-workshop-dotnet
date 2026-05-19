namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class GetByNameCatalogItemRequest : BaseRequest
{
    public string Name { get; init; }

    public GetByNameCatalogItemRequest(string name)
    {
        Name = name;
    }
}
