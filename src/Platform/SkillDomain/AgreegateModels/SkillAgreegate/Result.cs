using AppShareServices.Models;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class Result : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public DataType Data { get; set; }
    }
}
