using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using CatalogApplication.ClientServices;
using CatalogApplication.DTOs;
using CatalogDomain.AgreegateModels.CatalogAgreegate;

namespace CatalogApplication.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IMappingService _mappingService;
        private readonly IRepositoryService _repositoryService;
        private readonly ISpecificationClientService _specificationClientService;

        public CatalogService(IMappingService mappingService, IRepositoryService repositoryService, ISpecificationClientService specificationClientService)
        {
            _mappingService = mappingService;
            _repositoryService = repositoryService;
            _specificationClientService = specificationClientService;
        }

        public async Task<CatalogDto> Get(int id, bool isInclude)
        {
            var catalogExisting = _repositoryService.Find<Catalog>(id);
            var catalogDto = _mappingService.Map<CatalogDto>(catalogExisting);

            if (isInclude)
            {
                if (catalogExisting.SpecificationId != null)
                {
                    catalogDto.Specification = await _specificationClientService.Get((int)catalogExisting.SpecificationId);
                }

                // Find list specifications of addon
                if (catalogExisting.Addons != null && catalogExisting.Addons.Any())
                {
                    var addonSpecificationIds = catalogExisting.Addons.Where(x => x.SpecificationId.HasValue && x.SpecificationId > 0).Select(x => x.SpecificationId).ToArray();
                    var specificationDtos = await _specificationClientService.Get(addonSpecificationIds);

                    if (specificationDtos != null && specificationDtos.Any())
                    {
                        foreach (var addon in catalogDto.Addons)
                        {
                            var specificationDto = specificationDtos.FirstOrDefault(c => c.Id == addon.Specification.Id);
                            if (specificationDto != null)
                            {
                                addon.Specification = specificationDto;
                            }
                        }
                    }
                }
            }

            return catalogDto;
        }
    }
}
