using CatalogApplication.DTOs;
using Refit;

namespace CatalogApplication.ClientServices
{
    public interface ISpecificationClientService
    {
        /// <summary>
        /// https://localhost:7174/Specifications/1?isInclude=true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Get("/specifications/{id}")]
        public Task<SpecificationDto> Get(int id, bool isInclude);

        /// <summary>
        /// https://localhost:7174/Specifications?ids=1&ids=2
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        [Get("/specifications")]
        public Task<IEnumerable<SpecificationDto>> Get(int[] ids, bool isInclude);
    }
}
