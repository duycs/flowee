using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class SpecificationRule : Entity
    {
        public int SpecificationId { get; set; }
        public Specification Specification { get; set; }
        public int RuleId { get; set; }
        public Rule Rule { get; set; }
    }
}
