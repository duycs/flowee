using AppShareServices.DataAccess.Persistences;
using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.TimeKeepingAgreegate;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class Worker : Entity, IAggregateRoot
    {
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }

        [MaxLength(36)]
        [Required]
        public string Code { get; set; }

        [MaxLength(250)]
        public string? FullName { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }


        public virtual ICollection<WorkerGroup> WorkerGroups { get; set; }
        public virtual ICollection<WorkerRole> WorkerRoles { get; set; }
        public virtual ICollection<WorkerSkill> WorkerSkills { get; set; }
        public virtual ICollection<TimeKeeping> TimeKeepings { get; set; }

        public static Worker Create(string email, string? code, string? fullName, List<Role> roles, List<Group> groups, List<Skill> skills)
        {
            return new Worker()
            {
                Email = email,
                Code = !string.IsNullOrEmpty(code) ? code : GetCode(email),
                FullName = fullName ?? "",
                Roles = roles,
                Groups = groups,
                Skills = skills
            };
        }

        public static string GetCode(string email)
        {
            var code = email.Split("@")[0];
            return code;
        }
    }
}
