using AppShareServices.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationApplication.Commands
{
    public class ReviewResultOperationCommand : Command
    {
        public SubmitType SubmitType { get; set; }
        public string Comment { get; set; }
        public int[] RejectToSteps { get; set; }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }

    public enum SubmitType
    {
        Approved,
        Rejected
    }
}
