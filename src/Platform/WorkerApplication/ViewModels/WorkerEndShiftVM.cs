using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerApplication.ViewModels
{
    public class WorkerEndShiftVM
    {
        public int WorkerId { get; set; }
        public int ShiftId { get; set; }
        public DateTime DateEnded { get; set; }
    }
}
