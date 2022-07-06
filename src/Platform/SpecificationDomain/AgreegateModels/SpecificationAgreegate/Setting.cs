using System;
using System.ComponentModel.DataAnnotations;
using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Setting : Entity
    {
        public int SettingTypeId { get; set; }
        public SettingType SettingType {get;set;}

        [MaxLength(36)]
        public int Key { get; set; }
        public string Value { get; set; }

        [MaxLength(250)]
        public int Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int? SpecificationId { get; set; }
        public ICollection<Specification>? Specifications { get; set; }
        public ICollection<SpecificationSetting>? SpecificationSettings { get; set; }
    }
}

