using System;
using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class OperationRule : Entity
    {
        public Guid OperationId { get; set; }
        public int RuleId { get; set; }
        public Rule Rule { get; set; }
    }
}

