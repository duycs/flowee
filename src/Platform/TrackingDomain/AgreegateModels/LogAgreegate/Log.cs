using AppShareDomain.Models;
using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingDomain.AgreegateModels.LogAgreegate
{
    public class Log : Entity, IAggregateRoot
    {
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}
