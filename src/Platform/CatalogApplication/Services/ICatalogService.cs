using CatalogApplication.DTOs;

namespace CatalogApplication.Services
{
    public interface ICatalogService
    {
        Task<CatalogDto> Find(int id, bool isInclude);

        Task<IEnumerable<CatalogDto>> Find(int[] ids, bool isInclude);
    }
}
