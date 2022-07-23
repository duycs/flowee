using AppShareDomain.Models;
using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        public virtual ICollection<Role>? Roles { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }

        [JsonIgnore]
        public virtual ICollection<WorkerGroup>? WorkerGroups { get; set; }

        [JsonIgnore]
        public virtual ICollection<WorkerRole>? WorkerRoles { get; set; }

        public virtual ICollection<WorkerSkill>? WorkerSkills { get; set; }

        public static Worker Create(string email, string? code, string? fullName)
        {
            return new Worker()
            {
                Email = email,
                Code = !string.IsNullOrEmpty(code) ? code : GetCode(email),
                FullName = fullName ?? ""
            };
        }

        public static Worker Create(string email, string? code, string? fullName, List<Role>? roles, List<Group>? groups)
        {
            return new Worker()
            {
                Email = email,
                Code = !string.IsNullOrEmpty(code) ? code : GetCode(email),
                FullName = fullName ?? "",
                Roles = roles,
                Groups = groups,
            };
        }

        public Worker AddWorkerSkills(List<WorkerSkill> workerSkills)
        {
            WorkerSkills = workerSkills;
            return this;
        }

        /// <summary>
        /// optional update not null field
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="roles"></param>
        /// <param name="groups"></param>
        /// <param name="skills"></param>
        /// <returns></returns>
        public Worker PathUpdateWorker(string? fullName, List<Role>? roles, List<Group>? groups, List<WorkerSkill>? workerSkills)
        {
            if (!string.IsNullOrEmpty(fullName))
            {
                FullName = fullName;
            }

            if (roles is not null)
            {
                Roles = roles;
            }

            if (groups is not null)
            {
                Groups = groups;
            }

            if (workerSkills is not null)
            {
                WorkerSkills = workerSkills;
            }

            return this;
        }

        public static string GetCode(string email)
        {
            var code = email.Split("@")[0];
            return code;
        }
    }
}
