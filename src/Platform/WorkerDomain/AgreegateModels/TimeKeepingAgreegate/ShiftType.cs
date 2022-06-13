using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDomain.AgreegateModels.TimeKeepingAgreegate
{
    public class ShiftType : Enumeration
    {
        public static ShiftType Shift1 = new ShiftType(1, nameof(Shift1).ToLowerInvariant());
        public static ShiftType Shift2 = new ShiftType(2, nameof(Shift2).ToLowerInvariant());
        public static ShiftType Shift3 = new ShiftType(3, nameof(Shift3).ToLowerInvariant());
        public static ShiftType ShiftAbnormal = new ShiftType(3, nameof(ShiftAbnormal).ToLowerInvariant());

        public ShiftType(int id, string name) : base(id, name)
        {
        }
    }
}
