using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
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
        public virtual ICollection<SpecificationRule>? SpecificationRules { get; set; }


        public Specification SetInstruction()
        {
            Instruction = Build();
            return this;
        }

        /// <summary>
        /// Build specification from Settings
        /// Ex: 
        /// [Is Not] [And] [[12-Retouch] [Retouch:Retouch manual 100%]]
        /// [And] [[13-Shadow] [Shadow:Shadow 50%]]
        /// </summary>
        public string Build()
        {
            var instructionBuilder = new StringBuilder();

            if (Rules != null)
            {
                foreach (var rule in Rules)
                {
                    instructionBuilder.AppendLine(rule.Buid());
                }
            }

            return instructionBuilder.ToString();
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

            if (rules != null)
            {
                Rules = rules;
            }

            return this;
        }
    }
}

