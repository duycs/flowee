using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareDomain.DTOs.Skill
{
    public class MatrixSkillDto : DtoBase
    {
        public SkillDto Skill { get; set; }
        public EnumerationDto SpecificationSkillLevel { get; set; }
        public EnumerationDto WorkerSkillLevel { get; set; }

        /// <summary>
        /// Action worker should do
        /// </summary>
        public ActionDto Action { get; set; }

        /// <summary>
        /// EstimationTime in mini second
        /// </summary>
        public int EstimationTimeInMiniSecond { get; set; }

        /// <summary>
        /// Expect result
        /// </summary>
        public ResultDto Result { get; set; }
    }
}
