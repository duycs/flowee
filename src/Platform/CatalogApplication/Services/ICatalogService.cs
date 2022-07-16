using CatalogApplication.DTOs;
using CatalogDomain.AgreegateModels.CatalogAgreegate;

namespace CatalogApplication.Services
{
    public interface ICatalogService
    {
        Task<CatalogDto> Find(int id, bool isInclude);

        Task<IEnumerable<CatalogDto>> Find(int[] ids, bool isInclude);
        List<CatalogDto> Find(int pageNumber, int pageSize, string columnOrders, string searchValue, bool isInclude, out int totalRecords);
        Task<List<CatalogDto>> FindInclude(List<CatalogDto> catalogDtos);

        /// <summary>
        /// Build Description from Specification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task BuildCatalogDescription(int id);
    }
}
