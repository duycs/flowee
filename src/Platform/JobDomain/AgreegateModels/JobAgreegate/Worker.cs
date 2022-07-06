using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class Worker : Entity
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}
