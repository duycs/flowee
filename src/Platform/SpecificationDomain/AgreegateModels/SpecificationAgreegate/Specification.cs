using System;
using System.ComponentModel.DataAnnotations;
using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Specification : Entity
    {
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(36)]
        public string Code { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Setting>? Settings { get; set; }
        public virtual ICollection<SpecificationSetting>? SpecificationSettings { get; set; }

        public static Specification Create(string code, string? name, string? description, List<Setting>? settings)
        {
            return new Specification()
            {
                Code = code,
                Name = name ?? "",
                Description = description ?? "",
                Settings = settings
            };
        }

        public Specification PathUpdate(string? name, string? description, List<Setting>? settings)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
            }

            if (!string.IsNullOrEmpty(description))
            {
                Description = description;
            }

            if (settings != null)
            {
                Settings = settings;
            }

            return this;
        }
    }
}

