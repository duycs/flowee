using System;
using System.Data;
using AppShareDomain.Models;
using AppShareServices.Models;

namespace OperationDomain.AgreegateModels.OperationAgreegate
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
    }
}

