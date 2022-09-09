using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Specification;
using JobApplication.ViewModels;

namespace JobApplication.Services
{
    public interface IJobService
    {
        public Task<JobDto> Create(CreateJobVM createJobVM);

        /// <summary>
        /// Product => Specifications of Catalog and Addons
        /// </summary>
        /// <param name="productId">Product is a catalog with specifications is ready in production</param>
        /// <returns></returns>
        public Task<List<SpecificationDto>> GetSpecifications(int catalogId);

        /// <summary>
        /// Product => Specifications => Steps
        /// New step have Job, Skill and Operations of the Skill
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<List<StepDto>> GenerateSteps(int jobId, bool? isInclude);

        /// <summary>
        /// Generate steps then update the steps for job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<List<StepDto>> GenerateUpdateSteps(int jobId);

        /// <summary>
        /// Find workers active and matching skill of job then assign to steps
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public Task AutoAssignWorkers(int jobId);

        /// <summary>
        /// Fire fisrt step performed operations
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public Task Start(int jobId);

        /// <summary>
        /// Performed operation trigger tranformed step in job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="stepId"></param>
        public JobDto Transformed(int jobId, int stepId, out bool isChange);
    }
}
