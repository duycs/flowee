using System;
using System.ComponentModel.DataAnnotations;
using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Setting : Entity
    {
        public int SettingTypeId { get; set; }
        public SettingType SettingType { get; set; }

        [MaxLength(36)]
        public string Key { get; set; }
        public string? Value { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        public int Number { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<Specification>? Specifications { get; set; }
        public ICollection<SpecificationSetting>? SpecificationSettings { get; set; }

        public Setting Create(SettingType settingType, string key, string? value, string? name, string? description)
        {
            return new Setting()
            {
                SettingType = settingType,
                Key = key,
                Value = value,
                Name = name ?? "",
                Description = description ?? ""
            };
        }

        public Setting PathUpdate(SettingType? settingType, string? value, string? name, string? description)
        {
            if (settingType != null)
            {
                SettingType = settingType;
            }

            if (!string.IsNullOrEmpty(value))
            {
                Value = value;
            }

            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
            }

            if (!string.IsNullOrEmpty(description))
            {
                Description = description;
            }

            return this;
        }
    }
}

