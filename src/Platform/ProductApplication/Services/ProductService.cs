using AppShareDomain.DTOs.Product;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using ProductDomain.AgreegateModels.ProductAgreegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace ProductApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly ICatalogClientService _catalogClientService;
        private readonly IMappingService _mappingService;

        public ProductService(IRepositoryService repositoryService, ICatalogClientService catalogClientService,
            IMappingService mappingService)
        {
            _repositoryService = repositoryService;
            _catalogClientService = catalogClientService;
            _mappingService = mappingService;
        }

        /// <summary>
        /// Instruction description overall how to made this product
        /// Deductive from specifications of catalog
        /// </summary>
        public async Task BuildInstruction(int id)
        {
            var productExisting = _repositoryService.Find<Product>(id);
            if (productExisting is not null)
            {
                var productDto = await Find(id, true);
                if (productDto is not null && productDto.Catalog is not null)
                {
                    _repositoryService.Update(productExisting.SetInstruction(productDto.Catalog.Description));
                    _repositoryService.SaveChanges();
                }
            }
        }

        public async Task<ProductDto?> Find(int id, bool isInclude)
        {
            var productExisting = _repositoryService.Find<Product>(id, new ProductSpecification(isInclude));
            if (productExisting is null)
            {
                return null;
            }

            var productDto = _mappingService.Map<ProductDto>(productExisting);
            if (productDto is not null && productDto.CatalogId is not null && productDto.CatalogId > 0)
            {
                productDto.Catalog = await _catalogClientService.Get((int)productDto.CatalogId, isInclude);
            }

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>?> Find(int[] ids, bool isInclude)
        {
            var products = _repositoryService.List<Product>(ids, new ProductSpecification(isInclude));
            if (products is null)
            {
                return null;
            }

            var productDtos = _mappingService.Map<List<ProductDto>>(products);
            if (isInclude)
            {
                productDtos = await FindInclude(productDtos);
            }

            return productDtos;
        }

        public List<ProductDto> Find(int pageNumber, int pageSize, string columnOrders, string searchValue, bool isInclude, out int totalRecords)
        {
            var productSpecification = new ProductSpecification(isInclude, searchValue, columnOrders.ToColumnOrders());
            var pagedProducts = _repositoryService.Find<Product>(pageNumber, pageSize, productSpecification, out totalRecords).ToList();
            var productDtos = _mappingService.Map<List<ProductDto>>(pagedProducts);
            return FindInclude(productDtos).Result;
        }

        public async Task<List<ProductDto>> FindInclude(List<ProductDto> productDtos)
        {
            var catalogIds = productDtos.Where(c => c.CatalogId is not null && c.Id > 0).Select(x => x.CatalogId).Cast<int>().ToArray();
            if (catalogIds.Any())
            {
                var catalogDtos = await _catalogClientService.Get(catalogIds, true);
                if (catalogDtos is not null && catalogDtos.Any())
                {
                    productDtos.ForEach(productDto => productDto.Catalog = catalogDtos.FirstOrDefault(catalog => catalog.Id == productDto.CatalogId));
                }
            }

            return productDtos;
        }
    }
}
