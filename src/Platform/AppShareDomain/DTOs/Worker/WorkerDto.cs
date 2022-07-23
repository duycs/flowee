using AppShareDomain.DTOs.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareDomain.DTOs.Worker
{
    public class WorkerDto : DtoBase
    {
        public int? SkillId { get; set; }
        public SkillDto? Skill { get; set; }
    }
}
