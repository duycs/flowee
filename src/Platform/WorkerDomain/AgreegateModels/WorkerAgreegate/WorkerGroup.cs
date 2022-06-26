using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class WorkerGroup : Entity
    {
        [Required]
        [Column(Order = 0)]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        [Required]
        [Column(Order = 1)]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public bool IsActive { get; set; }
    }
}
