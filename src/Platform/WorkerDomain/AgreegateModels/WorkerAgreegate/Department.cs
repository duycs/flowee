﻿using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class Department : Entity
    {
        [MaxLength(36)]
        [Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Group>? Groups { get; set; }

        public Department()
        {
            Groups = new HashSet<Group>();
        }
    }
}
