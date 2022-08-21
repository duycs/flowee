using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Operation : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
