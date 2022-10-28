using AppShareServices.Models;
using OperationDomain.AgreegateModels.OperationAgreegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpecificationDomain.AgreegateModels.OperationAgreegate
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
        public State State { get; set; }

        public Request Request { get; set; }
        public Response Response { get; set; }


        /// <summary>
        /// Hanlde prepare inner data get from settings which valid rules
        /// </summary>
        public void PreProcess()
        {

        }

        /// <summary>
        /// Handle in processing
        /// </summary>
        /// <param name="isWaitOutsideAction"></param>
        /// <param name="request"></param>
        public void Processing(bool isWaitOutsideAction, Request request)
        {

        }

        /// <summary>
        /// Hanlde after process
        /// </summary>
        public Response PostProcess()
        {
            return new Response();
        }
    }
}
