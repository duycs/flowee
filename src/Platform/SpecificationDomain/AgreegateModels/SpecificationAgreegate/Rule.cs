using AppShareServices.Models;
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


        public virtual ICollection<Guid>? OperationIds { get; set; }

        [JsonIgnore]
        public virtual ICollection<OperationRule>? OperationRules { get; set; }

        /// <summary>
        /// Statify setting condition versus value
        /// Ex: number > value or boolean is true/false
        /// </summary>
        /// <returns></returns>
        public bool IsStatisfy()
        {
            if (Setting is null || Operator is null)
            {
                return false;
            }

            if (Operator.Equals(Operator.IsNotNull))
            {
                return Value is not null;
            }
            else if (Operator.Equals(Operator.IsNull))
            {
                return Value == null;
            }
            else if (Operator.Equals(Operator.IsEmpty))
            {
                return Value == string.Empty;
            }
            else if (Operator.Equals(Operator.IsNotEmpty))
            {
                return Value != string.Empty;
            }
            else if (Setting.Value is not null && Setting.SettingType is not null)
            {
                if (Setting.SettingType.Equals(SettingType.Number))
                {
                    if (Operator.Equals(Operator.GreaterThan))
                    {
                        return float.Parse(Setting.Value) > float.Parse(Value);
                    }
                    else if (Operator.Equals(Operator.LessThan))
                    {
                        return float.Parse(Setting.Value) < float.Parse(Value);
                    }
                    else if (Operator.Equals(Operator.Equal))
                    {
                        return float.Parse(Setting.Value) == float.Parse(Value);
                    }
                    else if (Operator.Equals(Operator.NotEqual))
                    {
                        return float.Parse(Setting.Value) != float.Parse(Value);
                    }
                }
                else if (Setting.SettingType.Equals(SettingType.Text))
                {
                    if (Operator.Equals(Operator.Contains))
                    {
                        return Setting.Value.Contains(Value);
                    }
                    else if (Operator.Equals(Operator.StartWith))
                    {
                        return Setting.Value.StartsWith(Value);
                    }
                    else if (Operator.Equals(Operator.EndWith))
                    {
                        return Setting.Value.EndsWith(Value);
                    }
                    else if (Operator.Equals(Operator.Equal))
                    {
                        return Setting.Value.Equals(Value);
                    }
                    else if (Operator.Equals(Operator.NotEqual))
                    {
                        return !Setting.Value.Equals(Value);
                    }
                }
            }

            return false;
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
