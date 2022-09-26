using AppShareServices.Commands;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;

namespace SpecificationApplication.Commands
{
    public class GetInstructionOperationCommand : CommandResponse<string>
    {
        public Specification Specification { get; set; }

        public GetInstructionOperationCommand(Specification specification)
        {
            Specification = specification;
        }

        public override bool IsValid()
        {
            return Specification.Rules != null;
        }
    }
}
