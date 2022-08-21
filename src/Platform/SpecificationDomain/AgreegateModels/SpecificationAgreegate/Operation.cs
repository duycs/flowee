using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    /// <summary>
    /// Operation
    /// - Many Settings => 1 Rule
    /// - And many Rules => 1 Operation
    /// - 1 Operation in list Operations of 1 Skill
    /// </summary>
    public class Operation : Entity
    {
        /// <summary>
        /// Reflection method name
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Rule> Rules { get; set; }

        [JsonIgnore]

        public virtual ICollection<OperationRule>? OperationRules { get; set; }

        /// <summary>
        /// Operation is statify all rules
        /// </summary>
        /// <returns></returns>
        public bool IsStatify()
        {
            return Rules.All(r => r.IsStatisfy());
        }
    }
}
