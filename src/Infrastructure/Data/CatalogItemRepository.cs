using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;

namespace Microsoft.eShopWeb.Infrastructure.Data;

public class CatalogItemRepository : EfRepository<CatalogItem>, ICatalogItemRepository
{
    public CatalogItemRepository(CatalogContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<CatalogItem>> GetByNameAsync(string name)
    {
        var spec = new CatalogItemNameSpecification(name);
        return await ListAsync(spec);
    }
}
