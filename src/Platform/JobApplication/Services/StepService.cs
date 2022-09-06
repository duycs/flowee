using System;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using JobDomain.AgreegateModels.JobAgreegate;

namespace JobApplication.Services
{
    public class StepService : IStepService
    {
        private readonly IMappingService _mappingService;
        private readonly IRepositoryService _repositoryService;

        public StepService(IMappingService mappingService, IRepositoryService repositoryService)
        {
            _mappingService = mappingService;
            _repositoryService = repositoryService;
        }

        public Task PostProcessing(int stepId)
        {
            throw new NotImplementedException();
        }

        public Task Preprocess(int stepId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processing performed operations
        /// </summary>
        /// <param name="stepId"></param>
        /// <param name="operationId"></param>
        /// <param name="isPerformedSuccess"></param>
        /// <returns></returns>
        public async Task Processing(int stepId, Guid operationId, bool statusPerformed, string? outputPerformed)
        {
            var step = _repositoryService.Find<Step>(s => s.Id == stepId);
            if (step is not null && step.StepOperations is not null && step.StepOperations.Any())
            {
                var stepOperation = step.StepOperations.FirstOrDefault(s => s.StepId == stepId && s.OperationId == operationId);
                if (stepOperation is not null)
                {
                    stepOperation.SetPerformed(statusPerformed, outputPerformed);
                    _repositoryService.Update(stepOperation);
                    _repositoryService.SaveChanges();
                }
            }
        }
    }
}

