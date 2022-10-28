using System;
using System.Collections.Generic;
using AppShareDomain.Models;

namespace SpecificationDomain.AgreegateModels.OperationAgreegate
{
    public class State : Enumeration
    {
        public static State Pending = new State(1, nameof(Pending).ToLowerInvariant());
        public static State Active = new State(2, nameof(Active).ToLowerInvariant());
        public static State Inprogress = new State(3, nameof(Inprogress).ToLowerInvariant());
        public static State SuccessFul = new State(4, nameof(SuccessFul).ToLowerInvariant());
        public static State Failed = new State(5, nameof(Failed).ToLowerInvariant());


        public State(int id, string name) : base(id, name)
        {
        }
    }
}

