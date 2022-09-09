using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareDomain.DTOs.Job
{
    public class StepOperationDto : DtoBase
    {
        public int StepId { get; set; }
        public StepDto Step { get; set; }

        public Guid OperationId { get; set; }

        public bool IsPerformed { get; set; }

        public string? OutputOperation { get; set; }
    }
}
