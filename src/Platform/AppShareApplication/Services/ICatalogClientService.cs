using AppShareDomain.DTOs.Catalog;
using Refit;

namespace AppShareServices.Services
{
    public interface ICatalogClientService
    {
        /// <summary>
        /// https://localhost:7174/Catalogs/1?isInclude=true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Get("/catalogs/{id}")]
        public Task<CatalogDto> Get(int id, bool isInclude);

        /// <summary>
        /// https://localhost:7174/Catalogs?ids=1&ids=2
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        [Get("/catalogs")]
        public Task<IEnumerable<CatalogDto>> Get(int[] ids, bool isInclude);
    }
}
