using AppShareDomain.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Operator : Enumeration
    {
        public static Operator Equal = new Operator(1, nameof(Equal).ToLowerInvariant());
        public static Operator NotEqual = new Operator(2, nameof(NotEqual).ToLowerInvariant());
        public static Operator In = new Operator(3, nameof(In).ToLowerInvariant());
        public static Operator NotIn = new Operator(4, nameof(NotIn).ToLowerInvariant());
        public static Operator IsEmpty = new Operator(5, nameof(IsEmpty).ToLowerInvariant());
        public static Operator IsNotEmpty = new Operator(6, nameof(IsNotEmpty).ToLowerInvariant());
        public static Operator IsNull = new Operator(7, nameof(IsNull).ToLowerInvariant());
        public static Operator IsNotNull = new Operator(8, nameof(IsNotNull).ToLowerInvariant());
        public static Operator LessThan = new Operator(9, nameof(LessThan).ToLowerInvariant());
        public static Operator GreaterThan = new Operator(10, nameof(GreaterThan).ToLowerInvariant());
        public static Operator StartWith = new Operator(11, nameof(StartWith).ToLowerInvariant());
        public static Operator EndWith = new Operator(12, nameof(EndWith).ToLowerInvariant());
        public static Operator Contains = new Operator(13, nameof(Contains).ToLowerInvariant());

        public Operator(int id, string name) : base(id, name)
        {
        }
    }
}

