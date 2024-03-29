﻿using AppShareDomain.Models;
using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class Skill : Entity, IAggregateRoot
    {
        [MaxLength(36)]
        [Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public virtual ICollection<Guid> OperationIds { get; set; }
    }
}
