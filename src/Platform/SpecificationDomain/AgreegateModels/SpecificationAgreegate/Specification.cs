using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using AppShareDomain.Models;
using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Specification : Entity, IAggregateRoot
    {
        [MaxLength(250)]
        public string? Name { get; set; }

        [MaxLength(36)]
        public string Code { get; set; }

        public string? Instruction { get; set; }

        public virtual ICollection<Rule>? Rules { get; set; }

        [JsonIgnore]
        public virtual ICollection<SpecificationRule>? SpecificationRules { get; set; }


        /// <summary>
        /// Build specification from Settings
        /// Ex: 
        /// [Is Not] [And] [[12-Retouch] [Retouch:Retouch manual 100%]]
        /// [And] [[13-Shadow] [Shadow:Shadow 50%]]
        /// </summary>
        public Specification BuildInstruction()
        {
            var instructionBuilder = new StringBuilder();

            if (Rules is not null)
            {
                // Ignore Condition of first rule
                for (int i = 0; i < Rules.Count; ++i)
                {
                    var rule = Rules.ElementAt(i);
                    if (i > 0)
                    {
                        instructionBuilder.AppendLine(rule.Buid());
                    }
                    else
                    {
                        instructionBuilder.AppendLine(rule.Buid(true));
                    }
                }
            }

            Instruction = instructionBuilder.ToString();
            return this;
        }

        public static Specification Create(string code, string? name, List<Rule>? rules)
        {
            return new Specification()
            {
                Code = code,
                Name = name ?? "",
                Rules = rules
            };
        }

        public Specification PathUpdate(string? name, List<Rule>? rules)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
            }

            if (rules is not null)
            {
                Rules = rules;
            }

            return this;
        }
    }
}

