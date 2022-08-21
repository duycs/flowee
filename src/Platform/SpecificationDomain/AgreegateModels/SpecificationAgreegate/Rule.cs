﻿using AppShareServices.Models;
using System.Text;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual ICollection<Specification>? Specifications { get; set; }

        [JsonIgnore]
        public virtual ICollection<SpecificationRule>? SpecificationRules { get; set; }

        public virtual ICollection<Operation>? Operations { get; set; }

        [JsonIgnore]

        public virtual ICollection<OperationRule>? OperationRules { get; set; }

        /// <summary>
        /// Statify setting condition versus value
        /// Ex: number > value or boolean is true/false
        /// </summary>
        /// <returns></returns>
        public bool IsStatisfy()
        {
            // TODO
            return true;
        }


        /// <summary>
        /// Ex: [Is Not] [And] [[12-Retouch] [Retouch:Retouch manual 100%]]
        /// </summary>
        /// <returns></returns>
        public string Buid(bool isfirstLine = false)
        {
            var ruleAsText = new StringBuilder();
            if (IsNot)
            {
                ruleAsText.Append($" [{"Is Not"}]");
            }

            // Ignore condition first line
            if (!isfirstLine)
            {
                if (Condition is not null)
                {
                    ruleAsText.Append($" [{Condition.Name}]");
                }
            }

            if (Setting is not null)
            {
                ruleAsText.Append($" [{Setting.BuildInstruction()}]");
            }

            return ruleAsText.ToString();
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
