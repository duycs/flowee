using System;
using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    /// <summary>
    /// Performed an action to Operation return output value compair to Condition
    /// FromStep valid Condition to transition ToStep
    /// Ex: A step valid Condition 1, transition to B step
    /// A step valid Condition 2, transition to C step
    /// </summary>
    public class Transition : Entity
    {
        public int FromStepId { get; set; }
        public Step FromStep { get; set; }

        public string Condition { get; set; }

        public int ToStepId { get; set; }
        public Step ToStep { get; set; }

        /// <summary>
        /// Next, Back, Retry
        /// </summary>
        public TransitionType TransitionType { get; set; }

        public bool IsValidCondition(string outputOperation)
        {
            return Condition == outputOperation;
        }

        public Step To()
        {
            return ToStep;
        }
    }
}

