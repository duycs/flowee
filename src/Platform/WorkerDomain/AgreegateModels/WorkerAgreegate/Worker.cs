using AppShareServices.DataAccess.Persistences;
using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class Worker : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public Department Department { get; set; }
    }
}
