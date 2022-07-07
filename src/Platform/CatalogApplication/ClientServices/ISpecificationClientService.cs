using CatalogApplication.DTOs;
using Refit;

namespace CatalogApplication.ClientServices
{
    public interface ISpecificationClientService
    {
        [Get("/specifications/{id}")]
        public Task<SpecificationDto> Get(int id);

        [Get("/specifications?id={ids}")]
        public Task<IEnumerable<SpecificationDto>> Get(int?[]? ids);
    }
}
