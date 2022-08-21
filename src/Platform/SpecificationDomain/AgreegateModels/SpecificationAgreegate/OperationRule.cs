using System;
using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class OperationRule : Entity
    {
        public int OperationId { get; set; }
        public Operation Operation { get; set; }
        public int RuleId { get; set; }
        public Rule Rule { get; set; }
    }
}

