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

        public int? Number { get; set; }

        [MaxLength(250)]
        public string? Name { get; set; }

        public string? Value { get; set; }


        public ICollection<Rule>? Rules { get; set; }

        public ICollection<RuleSetting>? RuleSettings { get; set; }

        /// <summary>
        /// Ex: [12-Retouch] [Retouch:Retouch manual 100%]
        /// </summary>
        /// <returns></returns>
        public string GetInstruction()
        {
            return $"[{Number}-{Key}] [{Name}:{Value}]";
        }

        public Setting Create(SettingType settingType, string key, string? value, string? name)
        {
            return new Setting()
            {
                SettingType = settingType,
                Key = key,
                Value = value,
                Name = name ?? "",
            };
        }

        public Setting PathUpdate(SettingType? settingType, string? value, string? name)
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

            return this;
        }


    }
}

