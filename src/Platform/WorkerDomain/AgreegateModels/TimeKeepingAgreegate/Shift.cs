using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDomain.AgreegateModels.TimeKeepingAgreegate
{
    public class Shift : Entity
    {
        public ShiftType ShiftType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
