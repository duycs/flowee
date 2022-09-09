namespace AppShareDomain.DTOs.Job
{
    public class TransitionDto : DtoBase
    {
        public int FromStepId { get; set; }
        public StepDto FromStep { get; set; }

        public string Condition { get; set; }

        public int ToStepId { get; set; }
        public StepDto ToStep { get; set; }

        /// <summary>
        /// Next, Back, Retry
        /// </summary>
        public EnumerationDto TransitionType { get; set; }

        /// <summary>
        /// Count number of transition step
        /// </summary>
        public int Count { get; set; }
    }
}
