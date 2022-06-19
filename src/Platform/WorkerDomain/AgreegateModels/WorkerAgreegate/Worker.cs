using AppShareServices.DataAccess.Persistences;
using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public static Worker Create(string email, string? code, string? fullName, int? roleId)
        {
            return new Worker()
            {
                Email = email,
                Code = !string.IsNullOrEmpty(code) ? code : GetCode(email),
                FullName = fullName ?? "",
                RoleId = roleId ?? 0,
            };
        }

        public static string GetCode(string email)
        {
            var code = email.Split("@")[0];
            return code;
        }
    }
}
