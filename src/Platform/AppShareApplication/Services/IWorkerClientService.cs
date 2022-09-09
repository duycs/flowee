using AppShareDomain.DTOs.Worker;
using Refit;

namespace AppShareApplication.Services
{
    public interface IWorkerClientService
    {
        [Get("/workers/active")]
        public Task<List<WorkerDto>> GetWorkersActive(int[]? skillIds, bool? isInclude);
    }
}
