using System;
using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    /// <summary>
    /// 
    /// </summary>
    public class Transition : Entity
    {
        public int FromStepId { get; set; }
        public Step FromStep { get; set; }

        public bool Condition { get; set; }

        public int ToStepId { get; set; }
        public Step ToStep { get; set; }

        /// <summary>
        /// Next, Back, Retry
        /// </summary>
        public TransitionType TransitionType { get; set; }

    }
}

