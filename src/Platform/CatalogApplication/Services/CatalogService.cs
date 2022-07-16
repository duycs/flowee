using AppShareDomain.DTOs.Catalog;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using CatalogApplication.ClientServices;
using CatalogDomain.AgreegateModels.CatalogAgreegate;
using System.Text;

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

        public async Task BuildCatalogDescription(int id)
        {
            var catalogExisting = _repositoryService.Find<Catalog>(id, new CatalogSpecification(true));
            if (catalogExisting is not null)
            {
                var specifications = await _specificationClientService.Get(catalogExisting.GetSpecificationIds(), true);
                var instructions = specifications.Where(s => s.Instruction is not null).Select(s => s.Instruction).ToList();
                var descriptionBuilder = new StringBuilder();
                foreach (var instruction in instructions)
                {
                    descriptionBuilder.AppendLine(instruction);
                }
                _repositoryService.Update(catalogExisting.SetDescription(descriptionBuilder.ToString()));
                _repositoryService.SaveChanges();
            }
        }

        public async Task<CatalogDto> Find(int id, bool isInclude)
        {
            var catalogExisting = _repositoryService.Find<Catalog>(id, new CatalogSpecification(isInclude));
            var catalogDto = _mappingService.Map<CatalogDto>(catalogExisting);

            if (isInclude)
            {
                if (catalogDto.SpecificationId > 0)
                {
                    catalogDto.Specification = await _specificationClientService.Get(catalogDto.SpecificationId, isInclude);
                }

                // Find list specifications of addon
                if (catalogDto.Addons is not null && catalogDto.Addons.Any())
                {
                    var addonSpecificationIds = catalogDto.Addons.Where(x => x.SpecificationId.HasValue && x.SpecificationId > 0).Select(x => x.SpecificationId).Cast<int>().ToArray();
                    var specificationDtos = await _specificationClientService.Get(addonSpecificationIds, isInclude);

                    if (specificationDtos is not null && specificationDtos.Any())
                    {
                        catalogDto.Addons.ForEach(addon => addon.Specification = specificationDtos.FirstOrDefault(c => c.Id == addon.SpecificationId));
                    }
                }
            }

            return catalogDto;
        }

        public async Task<IEnumerable<CatalogDto>> Find(int[] ids, bool isInclude)
        {
            var catalogsExisting = _repositoryService.List<Catalog>(ids, new CatalogSpecification(isInclude));
            var catalogDtos = _mappingService.Map<List<CatalogDto>>(catalogsExisting);
            if (isInclude)
            {
                catalogDtos = await FindInclude(catalogDtos);
            }
            return catalogDtos;
        }

        public List<CatalogDto> Find(int pageNumber, int pageSize, string columnOrders, string searchValue, bool isInclude, out int totalRecords)
        {
            var catalogSpecification = new CatalogSpecification(isInclude, searchValue, columnOrders.ToColumnOrders());
            var pagedCatalogs = _repositoryService.Find<Catalog>(pageNumber, pageSize, catalogSpecification, out totalRecords).ToList();
            var catalogDtos = _mappingService.Map<List<CatalogDto>>(pagedCatalogs);
            return FindInclude(catalogDtos).Result;
        }

        public async Task<List<CatalogDto>> FindInclude(List<CatalogDto> catalogDtos)
        {
            var catalogSpecificationIds = catalogDtos.Where(c => c.SpecificationId > 0).Select(c => c.SpecificationId).ToArray();
            if (catalogSpecificationIds.Any())
            {
                var catalogSpecificationDtos = await _specificationClientService.Get(catalogSpecificationIds, true);
                if (catalogSpecificationDtos is not null && catalogSpecificationDtos.Any())
                {
                    catalogDtos.ForEach(catalog => catalog.Specification = catalogSpecificationDtos.FirstOrDefault(c => c.Id == catalog.SpecificationId));
                }
            }

            foreach (var catalogDto in catalogDtos)
            {
                // Find list specifications of addon
                if (catalogDto.Addons is not null && catalogDto.Addons.Any())
                {
                    var addonSpecificationIds = catalogDto.Addons.Where(x => x.SpecificationId.HasValue && x.SpecificationId > 0).Select(x => x.SpecificationId).Cast<int>().ToArray();
                    var specificationDtos = await _specificationClientService.Get(addonSpecificationIds, true);

                    if (specificationDtos is not null && specificationDtos.Any())
                    {
                        catalogDto.Addons.ForEach(addon => addon.Specification = specificationDtos.FirstOrDefault(c => c.Id == addon.SpecificationId));
                    }
                }
            }

            return catalogDtos;
        }
    }
}
