﻿using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class Group : Entity
    {
        [MaxLength(36)]
        [Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Worker>? Workers { get; set; }
    }
}
