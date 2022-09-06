using System;
using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class StepOperation : Entity
    {
        public int StepId { get; set; }
        public Step Step { get; set; }

        public Guid OperationId { get; set; }

        public bool IsPerformed { get; set; }

        public string? OutputOperation { get; set; }

        public StepOperation SetPerformed(bool isSucess, string? outputPerformed)
        {
            IsPerformed = isSucess;
            OutputOperation = outputPerformed;
            return this;
        }
    }
}

