using AppShareDomain.DTOs.Catalog;

namespace JobApplication.Services
{
    public class CatalogClientService : ICatalogClientService
    {
        public async Task<CatalogDto> Get(int id, bool isInclude)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CatalogDto>> Get(int[] ids, bool isInclude)
        {
            throw new NotImplementedException();
        }
    }
}
