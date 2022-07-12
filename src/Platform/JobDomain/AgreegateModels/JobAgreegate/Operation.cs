using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    /// <summary>
    /// Have 4 steps and 4 workers need to do in a Job, the parallel scenario example:
    /// 1. First time  worker 1 do Step 1, output of step 1 is input of step 2. Scenario: Step 1, WorkerI
    /// 2. Worker 2 do Step 2 and worker 3 do step 3 at the same time, output of steps 2 and 3 is input of step 4
    /// 3. Worker 4 do step 4 at final, output of step 4 is final result.
    /// </summary>
    public class Operation : Entity
    {
        public ICollection<Step> Steps { get; set; }

        public int? StructTypeId { get; set; }
        public StructType? StructType { get; set; }

        public bool IsAsync { get; set; }
        public int OrderNumber { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string? Input { get; set; }
        public string? Output { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        public List<Step> SortSteps()
        {
            return Steps.OrderBy(s => s.OrderNumber).ToList();
        }
    }
}
