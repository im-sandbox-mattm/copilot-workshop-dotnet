using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using MinimalApi.Endpoint;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

/// <summary>
/// Get a Catalog Item by Name
/// </summary>
public class CatalogItemGetByNameEndpoint : IEndpoint<IResult, GetByNameCatalogItemRequest, IRepository<CatalogItem>>
{
    private readonly IUriComposer _uriComposer;

    public CatalogItemGetByNameEndpoint(IUriComposer uriComposer)
    {
        _uriComposer = uriComposer;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("api/catalog-items/by-name/{name}",
            async (string name, IRepository<CatalogItem> itemRepository) =>
            {
                return await HandleAsync(new GetByNameCatalogItemRequest(name), itemRepository);
            })
            .Produces<GetByNameCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
    }

    public async Task<IResult> HandleAsync(GetByNameCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
        var response = new GetByNameCatalogItemResponse(request.CorrelationId());

        var spec = new CatalogItemNameSpecification(request.Name);
        var item = await itemRepository.FirstOrDefaultAsync(spec);
        if (item is null)
            return Results.NotFound();

        response.CatalogItem = new CatalogItemDto
        {
            Id = item.Id,
            CatalogBrandId = item.CatalogBrandId,
            CatalogTypeId = item.CatalogTypeId,
            Description = item.Description,
            Name = item.Name,
            PictureUri = _uriComposer.ComposePicUri(item.PictureUri),
            Price = item.Price
        };
        return Results.Ok(response);
    }
}
