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
        public string Email { get; set; }
        public string Code { get; set; }
        public string? FullName { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<GroupDto> Groups { get; set; }
        public List<WorkerSkillDto> WorkerSkills { get; set; }
    }
}
