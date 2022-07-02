using AppShareDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerDomain.AgreegateModels.CustomerAgreegate
{
    public class Currency : Enumeration
    {
        public static Currency VND = new Currency(0, nameof(VND).ToLowerInvariant());
        public static Currency USD = new Currency(1, nameof(USD).ToLowerInvariant());
        public static Currency Euro = new Currency(2, nameof(Euro).ToLowerInvariant());

        public Currency(int id, string name) : base(id, name)
        {
        }
    }
}
