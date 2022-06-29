using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerDomain.AgreegateModels.TimeKeepingAgreegate
{
    /// <summary>
    /// Shif1, Shift2, Shift3 is normal shift, other is abNormal shift
    /// </summary>
    public class Shift : Entity
    {
        [MaxLength(26)]
        [Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public TimeOnly TimeStart { get; set; }
        public TimeOnly TimeEnd { get; set; }
        public bool IsNormal { get; set; }
    }
}
