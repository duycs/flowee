using AppShareDomain.DTOs.Catalog;
using AppShareDomain.DTOs.Specification;

namespace SpecificationApplication.Services
{
    public interface ISpecificationService
    {
        Task<SpecificationDto?> Find(int id, bool isInclude);
        Task<IEnumerable<SpecificationDto>?> Find(int[] ids, bool isInclude);
        List<SpecificationDto> Find(int pageNumber, int pageSize, string columnOrders, int[] ids, string searchValue, bool isInclude, out int totalRecords);
        Task<List<SpecificationDto>> FindInclude(List<SpecificationDto> workerDtos);
        List<OperationDto>? GetOperations(int id, bool isInclude);
    }
}
