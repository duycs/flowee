﻿using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationDomain.AgreegateModels.OperationAgreegate
{
    public class Request : Entity
    {
        public string Message { get; set; }
        public string Data { get; set; }
    }
}
