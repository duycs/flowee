using AppShareDomain.DTOs.Catalog;
using AppShareDomain.DTOs.Skill;
using AppShareDomain.DTOs.Specification;

namespace SpecificationApplication.Services
{
    public interface ISpecificationService
    {
        Task<SpecificationDto?> Find(int id, bool isInclude);
        Task<IEnumerable<SpecificationDto>?> Find(int[] ids, bool isInclude);
        List<SpecificationDto> Find(int pageNumber, int pageSize, string columnOrders, int[] ids, string searchValue, bool isInclude, out int totalRecords);
        Task<List<SpecificationDto>> FindInclude(List<SpecificationDto> workerDtos);

        /// <summary>
        /// Get Operation need to do Specification
        /// </summary>
        /// <param name="specificationId"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        List<OperationDto>? GetOperations(int specificationId, bool isInclude);

        /// <summary>
        /// Get Skills need to do Specification
        /// </summary>
        /// <param name="specificationId"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        Task<List<SkillDto>>? GetSkills(int specificationId, bool isInclude);
    }
}
