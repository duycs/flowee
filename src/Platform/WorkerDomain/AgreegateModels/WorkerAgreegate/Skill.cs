using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class Skill : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
