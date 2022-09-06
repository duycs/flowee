using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Specification;

namespace JobApplication.Services
{
    public interface IJobService
    {
        public Task AutoAssignWorkersToJob(int jobId);

        public Task AutoAssignWorkerToStep(int jobId, int stepId);

        public Task Start(int jobId);

        /// <summary>
        /// Trigger step transformed
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="stepId"></param>
        public void Transformed(int jobId, int stepId);

        /// <summary>
        /// Generate steps then update the steps for job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task UpdateSteps(int jobId);

        /// <summary>
        /// Product => Specifications => Steps
        /// New step have Job, Skill and Operations of the Skill
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<List<StepDto>> GenerateSteps(int jobId, bool? isInclude);

        /// <summary>
        /// Product => Specifications of Catalog and Addons
        /// </summary>
        /// <param name="productId">Product is a catalog with specifications is ready in production</param>
        /// <returns></returns>
        public Task<List<SpecificationDto>> GetSpecifications(int catalogId);
    }
}
