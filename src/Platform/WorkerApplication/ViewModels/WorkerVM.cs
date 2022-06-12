using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerApplication.ViewModels
{
    public class WorkerVM
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public Department Department { get; set; }
    }
}
