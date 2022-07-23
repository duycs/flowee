using AppShareDomain.Models;
using AppShareServices.DataAccess;
using AppShareServices.Extensions;
using SkillDomain.AgreegateModels.SkillAgreegate;
using SkillInfrastructure.DataAccess;
using System.Text.RegularExpressions;
using Action = SkillDomain.AgreegateModels.SkillAgreegate.Action;

namespace SkillAPI.SeedData
{
    public class SkillContextSeed : SeedDataBase
    {
        private SkillContext _context;
        private ILogger<SkillContextSeed> _logger;
        private IConfiguration _configuration;
        private IWebHostEnvironment _hostEnvironment;

        public SkillContextSeed(SkillContext context, ILogger<SkillContextSeed> logger,
            IWebHostEnvironment hostEnvironment, IConfiguration configuration, string contentRootPath, string seedDataFolder)
            : base(context, contentRootPath, seedDataFolder)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        // Seed to database
        public override async Task SeedAsync()
        {
            var policy = CreatePolicy(_logger, nameof(SkillContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!_context.WorkerSkillLevels.Any())
                {
                    var workerSkillLevels = GetWorkerSkillLevelFromFile();
                    await _context.WorkerSkillLevels.AddRangeAsync(workerSkillLevels);
                    _context.SaveChanges();
                }

                if (!_context.SpecificationSkillLevels.Any())
                {
                    var specificationSkillLevels = GetSpecificationSkillLevelFromFile();
                    await _context.SpecificationSkillLevels.AddRangeAsync(specificationSkillLevels);
                    _context.SaveChanges();
                }

                if (!_context.Skills.Any())
                {
                    var skills = GetSkillFromFile();
                    await _context.Skills.AddRangeAsync(skills);
                    _context.SaveChanges();
                }

                if (!_context.Actions.Any())
                {
                    var actions = GetActionFromFile();
                    await _context.Actions.AddRangeAsync(actions);
                    _context.SaveChanges();
                }

                if (!_context.Results.Any())
                {
                    var results = GetResultFromFile();
                    await _context.Results.AddRangeAsync(results);
                    _context.SaveChanges();
                }

                if (!_context.MatrixSkills.Any())
                {
                    var matrixSkills = GetMatrixSkillFromFile();
                    await _context.MatrixSkills.AddRangeAsync(matrixSkills);
                    _context.SaveChanges();
                }

            });
        }

        private IEnumerable<WorkerSkillLevel> GetWorkerSkillLevelFromFile()
        {
            string csvFile = GetPathToFile("WorkerSkillLevels.csv");
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<WorkerSkillLevel>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => CreateWorkerSkillLevel(x, ref id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }

        private WorkerSkillLevel CreateWorkerSkillLevel(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("WorkerSkillLevel is null or empty");
            }

            return new WorkerSkillLevel(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private IEnumerable<SpecificationSkillLevel> GetSpecificationSkillLevelFromFile()
        {
            string csvFile = GetPathToFile("SpecificationSkillLevels.csv");
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<SpecificationSkillLevel>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => CreateSpecificationSkillLevel(x, ref id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }

        private SpecificationSkillLevel CreateSpecificationSkillLevel(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("SpecificationSkillLevel is null or empty");
            }

            return new SpecificationSkillLevel(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private IEnumerable<Skill> GetSkillFromFile()
        {
            string csvFile = GetPathToFile("Skills.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Skill>();
            }

            string[] requiredHeaders = { "Code", "Name", "Description" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateSkill(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }


        private Skill CreateSkill(string[] column, string[] headers)
        {
            var skill = new Skill()
            {
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return skill;
        }


        private IEnumerable<Action> GetActionFromFile()
        {
            string csvFile = GetPathToFile("Actions.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Action>();
            }

            string[] requiredHeaders = { "Code", "Name", "Description" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateAction(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }


        private Action CreateAction(string[] column, string[] headers)
        {
            var action = new Action()
            {
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return action;
        }

        private IEnumerable<Result> GetResultFromFile()
        {
            string csvFile = GetPathToFile("Results.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Result>();
            }

            string[] requiredHeaders = { "Code", "Name", "Description" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateResult(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }


        private Result CreateResult(string[] column, string[] headers)
        {
            var result = new Result()
            {
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return result;
        }


        private IEnumerable<MatrixSkill> GetMatrixSkillFromFile()
        {
            string csvFile = GetPathToFile("MatrixSkills.csv");

            if (!File.Exists(csvFile))
            {
                return new List<MatrixSkill>();
            }

            string[] requiredHeaders = { "SkillId", "SpecificationSkillLevelId", "WorkerSkillLevelId", "ActionId", "EstimationTimeInMiniSecond", "ResultId" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateMatrixSkill(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x is not null);
        }


        private MatrixSkill CreateMatrixSkill(string[] column, string[] headers)
        {
            var skill = _context.Skills.Find(int.Parse(column[Array.IndexOf(headers, "SkillId".ToLower())].Trim('"').Trim()));
            var workerSkillLevel = _context.WorkerSkillLevels.Find(int.Parse(column[Array.IndexOf(headers, "WorkerSkillLevelId".ToLower())].Trim('"').Trim()));
            var specificationSkillLevel = _context.SpecificationSkillLevels.Find(int.Parse(column[Array.IndexOf(headers, "specificationSkillLevelId".ToLower())].Trim('"').Trim()));
            var action = _context.Actions.Find(int.Parse(column[Array.IndexOf(headers, "ActionId".ToLower())].Trim('"').Trim()));
            var result = _context.Results.Find(int.Parse(column[Array.IndexOf(headers, "ResultId".ToLower())].Trim('"').Trim()));

            if (skill is null)
            {
                throw new Exception("Skill is null or empty");
            }

            if (workerSkillLevel is null)
            {
                throw new Exception("WorkerSkillLevel is null or empty");
            }

            if (specificationSkillLevel is null)
            {
                throw new Exception("SpecificationSkillLevel is null or empty");
            }

            if (action is null)
            {
                throw new Exception("Action is null or empty");
            }


            if (result is null)
            {
                throw new Exception("Result is null or empty");
            }

            var matrixSkill = new MatrixSkill()
            {
                Skill = skill,
                WorkerSkillLevel = workerSkillLevel,
                SpecificationSkillLevel = specificationSkillLevel,
                Action = action,
                Result = result,
                EstimationTimeInMiniSecond = int.Parse(column[Array.IndexOf(headers, "EstimationTimeInMiniSecond".ToLower())].Trim('"').Trim()),
                DateCreated = DateTime.UtcNow,
            };

            return matrixSkill;
        }

    }
}
