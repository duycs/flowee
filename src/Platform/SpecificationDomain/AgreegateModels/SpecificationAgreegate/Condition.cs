using System;
using AppShareDomain.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Condition : Enumeration
    {
        public static Condition And = new Condition(1, nameof(And).ToLowerInvariant());
        public static Condition Or = new Condition(2, nameof(Or).ToLowerInvariant());
        public static Condition Xor = new Condition(3, nameof(Xor).ToLowerInvariant());

        public Condition(int id, string name) : base(id, name)
        {
        }
    }
}

