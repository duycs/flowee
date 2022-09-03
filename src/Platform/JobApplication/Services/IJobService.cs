using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Specification;

namespace JobApplication.Services
{
    public interface IJobService
    {
        public Task AssignWorkersToJob(int jobId, int[] workerIds);

        public Task AssignWorkerToStep(int stepId, int workerId);

        public Task SetNextSteps(int[] nextStepIds);

        public Task SetLastSteps(int[] lastSteps);

        public Task UpdateJobStatus(EnumerationDto jobStatus);

        /// <summary>
        /// Generate steps then update the steps for job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task UpdateSteps(int jobId, int productId);

        /// <summary>
        /// Product => Specifications => Steps
        /// New step have Job, Skill and Operations of the Skill
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<List<StepDto>> GenerateSteps(int jobId, int productId, bool? isInclude);

        /// <summary>
        /// Product => Specifications of Catalog and Addons
        /// </summary>
        /// <param name="productId">Product is a catalog with specifications is ready in production</param>
        /// <returns></returns>
        public Task<List<SpecificationDto>> GetSpecifications(int productId);
    }
}
