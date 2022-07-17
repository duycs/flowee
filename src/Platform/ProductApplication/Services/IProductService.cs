using AppShareDomain.DTOs.Product;

namespace ProductApplication.Services
{
    public interface IProductService
    {
        Task<ProductDto?> Find(int id, bool isInclude);

        Task<IEnumerable<ProductDto>?> Find(int[] ids, bool isInclude);
        List<ProductDto> Find(int pageNumber, int pageSize, string columnOrders, string searchValue, bool isInclude, out int totalRecords);
        Task<List<ProductDto>> FindInclude(List<ProductDto> catalogDtos);

        /// <summary>
        /// Build Instruction from Specification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task BuildInstruction(int id);
    }
}
