using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class Group : Entity
    {
        [MaxLength(36)]
        [Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public virtual ICollection<Worker>? Workers { get; set; }

        [JsonIgnore]
        public virtual ICollection<WorkerGroup>? WorkerGroups { get; set; }
    }
}
