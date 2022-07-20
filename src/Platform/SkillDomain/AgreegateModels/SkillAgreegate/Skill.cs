﻿using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class Skill : Entity
    {
        [MaxLength(36)]
        [Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
