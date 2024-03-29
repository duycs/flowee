﻿using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class Step : Entity
    {
        /// <summary>
        /// Step of Job
        /// </summary>
        public int JobId { get; set; }
        public Job Job { get; set; }

        public int SkillId { get; set; }

        /// <summary>
        /// Skill => Operations
        /// </summary>
        public ICollection<Guid> OperationIds { get; set; } = new HashSet<Guid>();

        public ICollection<StepOperation>? StepOperations { get; set; }

        /// <summary>
        /// Text instruction all operations
        /// </summary>
        public string? Instruction { get; set; }

        /// <summary>
        /// Woker will be assign to this step
        /// </summary>
        public int? WorkerId { get; set; }

        /// <summary>
        /// Json input
        /// </summary>
        public string? Input { get; set; }

        /// <summary>
        /// Json output
        /// </summary>
        public string? Output { get; set; }

        public ICollection<Transition>? Transitions { get; set; }

        public int StateId { get; set; }
        public State State { get; set; }

        public bool IsCurrentState { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public static Step Create(int jobId, int skillId, List<Guid>? operationIds)
        {
            return new Step()
            {
                JobId = jobId,
                SkillId = skillId,
                OperationIds = operationIds,
                State = State.None
            };
        }

        public Step AssignWorker(int workerId)
        {
            WorkerId = workerId;
            return this;
        }

        public bool IsPerformedAllOperations()
        {
            if (StepOperations is null || !StepOperations.Any())
            {
                return false;
            }

            return StepOperations.All(o => o.IsPerformed);
        }

        public Step? GetOperationNotPerformed(Guid operationId)
        {
            if(StepOperations is null || !StepOperations.Any())
            {
                return null;
            }

            var stepOperation = StepOperations.FirstOrDefault(so => so.OperationId == operationId && !so.IsPerformed);
            if(stepOperation is null)
            {
                return null;
            }

            return stepOperation.Step;
        }

        /// <summary>
        /// This step is from step in Transitions
        /// </summary>
        public List<Step>? GetNextSteps()
        {
            if (Transitions is null || Transitions.Any())
            {
                return new List<Step>();
            }

            return Transitions.Where(t => t.FromStep.Id == this.Id).Select(t => t.ToStep).ToList();
        }

        /// <summary>
        /// This step is to step in Transitions
        /// </summary>
        public List<Step>? GetLastSteps()
        {
            if (Transitions is null || Transitions.Any())
            {
                return new List<Step>();
            }

            return Transitions.Where(t => t.ToStep.Id == this.Id).Select(t => t.FromStep).ToList();
        }
    }
}
