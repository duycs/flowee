using AppShareDomain.Models;
using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;

namespace TrackingDomain.AgreegateModels.JobAgreegate
{
    public class Note : Entity, IAggregateRoot
    {
        public int NoterId { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }
        public string Content { get; set; }

        public string ToString() => $"NoterId: {NoterId}. Title: {Title}. Content: {Content}";
    }
}
