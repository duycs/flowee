using System;
using System.Collections.Generic;
using AppShareDomain.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class OperationState : Enumeration
    {
        public static OperationState Pending = new OperationState(1, nameof(Pending).ToLowerInvariant());
        public static OperationState Active = new OperationState(2, nameof(Active).ToLowerInvariant());
        public static OperationState Inprogress = new OperationState(3, nameof(Inprogress).ToLowerInvariant());
        public static OperationState SuccessFul = new OperationState(4, nameof(SuccessFul).ToLowerInvariant());
        public static OperationState Failed = new OperationState(4, nameof(Failed).ToLowerInvariant());


        public OperationState(int id, string name) : base(id, name)
        {
        }
    }
}

