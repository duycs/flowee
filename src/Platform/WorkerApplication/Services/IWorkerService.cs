using AppShareDomain.DTOs.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerApplication.Services
{
    public interface IWorkerService
    {
        Task<WorkerDto?> Find(int id, bool isInclude);
        Task<IEnumerable<WorkerDto>?> Find(int[] ids, bool isInclude);
        List<WorkerDto> Find(int pageNumber, int pageSize, string columnOrders, string searchValue, bool isInclude, out int totalRecords);
        Task<List<WorkerDto>> FindInclude(List<WorkerDto> workerDtos);

    }
}
