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

        /// <summary>
        /// Count total performed operations
        /// Validate results of operations
        /// </summary>
        /// <param name="outputOperation"></param>
        /// <returns></returns>
        public bool IsValid()
        {
            if (FromStep.StepOperations is null || !FromStep.StepOperations.Any())
            {
                return false;
            }

            var isAllPerformed = FromStep.StepOperations.All(s => s.IsPerformed);
            var allOutputPerformed = FromStep.StepOperations.Select(s => s.OutputOperation);
            string outputConditions = allOutputPerformed.Any() ? String.Join(",", allOutputPerformed) : "";

            if (isAllPerformed && outputConditions == Condition)
            {
                return true;
            }

            return false;
        }

        public Step To()
        {
            return ToStep;
        }
    }
}

