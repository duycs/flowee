using System;
namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class StepOperations
    {
        public int StepId { get; set; }
        public Step Step { get; set; }

        public int OperationId { get; set; }
    }
}

