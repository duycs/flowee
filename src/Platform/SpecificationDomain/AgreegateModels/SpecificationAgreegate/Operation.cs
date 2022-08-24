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
        /// Guid for public global function
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Function name for invoke
        /// </summary>
        public string Function { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public OperationState State { get; set; }

        public virtual ICollection<Rule> Rules { get; set; }

        [JsonIgnore]

        public virtual ICollection<OperationRule>? OperationRules { get; set; }

        /// <summary>
        /// Invoke function
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public Response Invoke(string function)
        {
            // send command to execute function
            return Response.Create(1, String.Empty);
        }

        public Operation UpdateState(OperationState state)
        {
            State = state;
            return this;
        }

        /// <summary>
        /// Operation is statify all rules
        /// </summary>
        /// <returns></returns>
        public bool IsStatify()
        {
            return Rules.All(r => r.IsStatisfy());
        }

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        public List<Setting> GetSettings(bool? isStatisfy)
        {
            if(!isStatisfy ?? false)
            {
                return Rules.Select(r => r.Setting).ToList();
            }

            return Rules.Where(r => r.IsStatisfy()).Select(r => r.Setting).ToList();
        }
    }
}
