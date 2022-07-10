using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Rule : Entity
    {
        public bool IsNot { get; set; }

        public int? ConditionId { get; set; }
        public Condition? Condition { get; set; }

        public int SettingId { get; set; }
        public Setting Setting { get; set; }

        public int? OperatorId { get; set; }
        public Operator? Operator { get; set; }

        public string Value { get; set; }

        public virtual ICollection<Setting>? Settings { get; set; }
        public virtual ICollection<RuleSetting>? RuleSettings { get; set; }

        public virtual ICollection<Specification>? Specifications { get; set; }
        public virtual ICollection<SpecificationRule>? SpecificationRules { get; set; }


        /// <summary>
        /// Ex: [Is Not] [And] [[12-Retouch] [Retouch:Retouch manual 100%]]
        /// </summary>
        /// <returns></returns>
        public string Buid()
        {
            var isNot = IsNot ? "Is Not" : "";
            return $"[{isNot}] [{Condition.Name}] [{Setting.GetInstruction()}]";
        }

        /// <summary>
        /// Ex: [Is Not] [And] [[12-Retouch] [Retouch:Retouch manual 100%]] [Equal] [100]
        /// </summary>
        /// <returns></returns>
        public string BuildWithQuery()
        {
            return $"{Buid()} [{Operator}] [{Value}]";
        }
    }
}
