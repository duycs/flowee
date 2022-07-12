using AppShareDomain.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    /// <summary>
    /// Collection Steps data struct stype
    /// </summary>
    public class StructType : Enumeration
    {
        public static StructType None = new StructType(1, nameof(None).ToLowerInvariant());
        public static StructType List = new StructType(2, nameof(List).ToLowerInvariant());
        public static StructType LinkedList = new StructType(3, nameof(LinkedList).ToLowerInvariant());
        public static StructType Queue = new StructType(4, nameof(Queue).ToLowerInvariant());
        public static StructType Stack = new StructType(5, nameof(Stack).ToLowerInvariant());
        public StructType(int id, string name) : base(id, name)
        {
        }
    }
}