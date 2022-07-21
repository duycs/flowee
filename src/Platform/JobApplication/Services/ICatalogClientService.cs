using AppShareDomain.DTOs.Catalog;
using Refit;

namespace JobApplication.Services
{
    public interface ICatalogClientService
    {

        [Get("/catalogs/{id}")]
        public Task<CatalogDto> Get(int id, bool isInclude);

        [Get("/catalogs")]
        public Task<IEnumerable<CatalogDto>> Get(int[] ids, bool isInclude);
    }
}
