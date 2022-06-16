using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class WorkerSkill : Entity
    {
        public Skill Skill { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public bool IsActive { get; set; }
        public bool IsPriority { get; set; }
    }
}
