using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Specification;

namespace JobApplication.Services
{
    public interface IJobService
    {
        /// <summary>
        /// Product => Specification => Steps
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<List<StepDto>> GenerateSteps(int jobId, int productId, bool isInclude);

        /// <summary>
        /// Product => Specifications of Catalog and Addons
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<List<SpecificationDto>> GetSpecifications(int productId);
    }
}
