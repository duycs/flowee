using AppShareDomain.Models;
using AppShareServices.DataAccess;
using AppShareServices.Extensions;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;
using SpecificationInfrastructure.DataAccess;
using System.Text.RegularExpressions;

namespace SpecificationAPI.SeedData
{
    public class SpecificationContextSeed : SeedDataBase
    {
        private readonly SpecificationContext _dbContext;
        private ILogger<SpecificationContextSeed> _logger;

        public SpecificationContextSeed(SpecificationContext dbContext, string contentRootPath, string seedDataFolder, ILogger<SpecificationContextSeed> logger) : base(dbContext, contentRootPath, seedDataFolder)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public override async Task SeedAsync()
        {
            var policy = CreatePolicy(_logger, nameof(SpecificationContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!_dbContext.Conditions.Any())
                {
                    var conditions = GetConditions();
                    if (conditions.Any())
                    {
                        await _dbContext.Conditions.AddRangeAsync(conditions);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.Operators.Any())
                {
                    var operators = GetOperators();
                    if (operators.Any())
                    {
                        await _dbContext.Operators.AddRangeAsync(operators);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.SettingTypes.Any())
                {
                    var settingTypes = GetSettingTypes();
                    if (settingTypes.Any())
                    {
                        await _dbContext.SettingTypes.AddRangeAsync(settingTypes);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.Settings.Any())
                {
                    var settings = GetSettingFromFile();
                    if (settings.Any())
                    {
                        await _dbContext.Settings.AddRangeAsync(settings);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.Rules.Any())
                {
                    var rules = GetRuleFromFile();
                    if (rules.Any())
                    {
                        await _dbContext.Rules.AddRangeAsync(rules);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.Specifications.Any())
                {
                    var specifications = GetSpecificationFromFile();
                    if (specifications.Any())
                    {
                        await _dbContext.Specifications.AddRangeAsync(specifications);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.SpecificationSkills.Any())
                {
                    var specificationSkills = GetSpecificationSkillFromFile();
                    if (specificationSkills.Any())
                    {
                        await _dbContext.SpecificationSkills.AddRangeAsync(specificationSkills);
                        _dbContext.SaveChanges();
                    }
                }
            });
        }

        private IEnumerable<Condition> GetConditions()
        {
            return Enumeration.GetAll<Condition>();
        }

        private IEnumerable<Operator> GetOperators()
        {
            return Enumeration.GetAll<Operator>();
        }

        private IEnumerable<SettingType> GetSettingTypes()
        {
            return Enumeration.GetAll<SettingType>();
        }

        private IEnumerable<Setting> GetSettingFromFile()
        {
            string csvFile = GetPathToFile("Settings.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Setting>();
            }

            string[] requiredHeaders = { "SettingType", "Key", "Number", "Name", "Value" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateSetting(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }

        private Setting CreateSetting(string[] column, string[] headers)
        {
            var setting = new Setting()
            {
                SettingType = Enumeration.FromDisplayName<SettingType>(CsvExtensions.GetColumnValueIgnoreCase(column, headers, "SettingType")),
                Key = column[Array.IndexOf(headers, "Key".ToLower())].Trim('"').Trim(),
                Number = int.Parse(column[Array.IndexOf(headers, "Number".ToLower())].Trim('"').Trim()),
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Value = column[Array.IndexOf(headers, "Value".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return setting;
        }

        private IEnumerable<Rule> GetRuleFromFile()
        {
            string csvFile = GetPathToFile("Rules.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Rule>();
            }

            string[] requiredHeaders = { "IsNot", "Condition", "SettingId", "Operator", "Value" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateRule(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }

        private Rule CreateRule(string[] column, string[] headers)
        {
            var setting = _dbContext.Settings.Find(int.Parse(column[Array.IndexOf(headers, "SettingId".ToLower())].Trim('"').Trim()));
            if (setting is null)
            {
                throw new Exception("Setting is null or empty");
            }

            var rule = new Rule()
            {
                IsNot = bool.Parse(column[Array.IndexOf(headers, "IsNot".ToLower())].Trim('"').Trim()),
                Condition = Enumeration.FromDisplayName<Condition>(CsvExtensions.GetColumnValueIgnoreCase(column, headers, "Condition")),
                Setting = setting,
                Operator = Enumeration.FromDisplayName<Operator>(CsvExtensions.GetColumnValueIgnoreCase(column, headers, "Operator")),
                Value = column[Array.IndexOf(headers, "Value".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return rule;
        }

        private IEnumerable<Specification> GetSpecificationFromFile()
        {
            string csvFile = GetPathToFile("Specifications.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Specification>();
            }

            string[] requiredHeaders = { "Name", "Code", "RuleIds" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateSpecification(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }

        private Specification CreateSpecification(string[] column, string[] headers)
        {
            var ruleIds = column[Array.IndexOf(headers, "RuleIds".ToLower())].Trim('"').Trim().Split(",").Select(int.Parse).ToList();
            var rules = _dbContext.Rules.Where(c => ruleIds.Contains(c.Id)).ToList();
            var specification = new Specification()
            {
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Rules = rules,
                DateCreated = DateTime.UtcNow,
            };

            return specification;
        }

        private IEnumerable<SpecificationSkill> GetSpecificationSkillFromFile()
        {
            string csvFile = GetPathToFile("SpecificationSkills.csv");

            if (!File.Exists(csvFile))
            {
                return new List<SpecificationSkill>();
            }

            string[] requiredHeaders = { "SpecificationId", "SkillId", "SkillLevelId" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateSpecificationSkill(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }

        private SpecificationSkill CreateSpecificationSkill(string[] column, string[] headers)
        {
            var specification = _dbContext.Specifications.FirstOrDefault(c => c.Id == int.Parse(column[Array.IndexOf(headers, "SpecificationId".ToLower())].Trim('"').Trim()));

            if (specification == null)
            {
                throw new Exception("Specification does not found");
            }

            var specificationSkill = new SpecificationSkill()
            {
                Specification = specification,
                SkillId = int.Parse(column[Array.IndexOf(headers, "SkillId".ToLower())].Trim('"').Trim()),
                SkillLevelId = int.Parse(column[Array.IndexOf(headers, "SkillLevelId".ToLower())].Trim('"').Trim()),
                DateCreated = DateTime.UtcNow,
            };

            return specificationSkill;
        }
    }
}