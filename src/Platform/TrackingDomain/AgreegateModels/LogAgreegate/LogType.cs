using System;
using AppShareDomain.Models;

namespace TrackingDomain.AgreegateModels.LogAgreegate
{
	public class LogType : Enumeration
    {
        public static LogType Comment = new LogType(1, nameof(Comment).ToLowerInvariant());
        public static LogType Note = new LogType(2, nameof(Note).ToLowerInvariant());
        public static LogType System = new LogType(2, nameof(System).ToLowerInvariant());

        public LogType(int id, string name) : base(id, name)
    {
    }
}
}

