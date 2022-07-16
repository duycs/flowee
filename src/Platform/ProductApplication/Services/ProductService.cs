using AppShareDomain.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApplication.Services
{
    public class ProductService : IProductService
    {
        public Task BuildInstruction(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> Find(int id, bool isInclude)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> Find(int[] ids, bool isInclude)
        {
            throw new NotImplementedException();
        }

        public List<ProductDto> Find(int pageNumber, int pageSize, string columnOrders, string searchValue, bool isInclude, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductDto>> FindInclude(List<ProductDto> catalogDtos)
        {
            throw new NotImplementedException();
        }
    }
}
