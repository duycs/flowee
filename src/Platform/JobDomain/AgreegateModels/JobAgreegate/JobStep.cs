using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class JobStep : Entity
    {
        public int? WorkerId { get; set; }
        public int? SkillId { get; set; }
        public int ProductId { get; set; }
        public JobStepStatus JobStepStatus { get; set; }

        public int JobId { get; set; }
        public Job? Job { get; set; }

        public string? Instruction { get; set; }

        /// <summary>
        /// Notes from Tracking Note Service
        /// </summary>
        public ICollection<int>? NoteIds { get; set; }
    }
}
