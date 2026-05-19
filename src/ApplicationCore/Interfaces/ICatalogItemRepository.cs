using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface ICatalogItemRepository : IRepository<CatalogItem>
{
    Task<IReadOnlyList<CatalogItem>> GetByNameAsync(string name);
}
