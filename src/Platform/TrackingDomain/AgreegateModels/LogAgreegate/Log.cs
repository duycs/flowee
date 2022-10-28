using AppShareDomain.Models;
using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;

namespace TrackingDomain.AgreegateModels.LogAgreegate
{
    public class Log : Entity, IAggregateRoot
    {
        public int LoggerId { get; set; }

        public LogType Type { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        public string Content { get; set; }

        public string ToString() => $"Type: {Type}, LoggerId: {LoggerId}. Title: {Title}. Content: {Content}";
    }
}
