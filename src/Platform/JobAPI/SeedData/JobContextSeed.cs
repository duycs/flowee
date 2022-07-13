using AppShareDomain.Models;
using AppShareServices.DataAccess;
using AppShareServices.Extensions;
using JobDomain.AgreegateModels.JobAgreegate;
using JobInfrastructure.DataAccess;
using System.Text.RegularExpressions;

namespace JobAPI.SeedData
{
    public class JobContextSeed : SeedDataBase
    {
        private readonly JobContext _dbContext;
        private ILogger<JobContextSeed> _logger;

        public JobContextSeed(JobContext dbContext, string contentRootPath, string seedDataFolder, ILogger<JobContextSeed> logger) : base(dbContext, contentRootPath, seedDataFolder)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public override async Task SeedAsync()
        {
            var policy = CreatePolicy(_logger, nameof(JobContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!_dbContext.JobStatus.Any())
                {
                    var jobStatus = GetJobStatusFromFile();
                    if (jobStatus.Any())
                    {
                        await _dbContext.JobStatus.AddRangeAsync(jobStatus);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.StepStatus.Any())
                {
                    var stepStatus = GetStepStatusFromFile();
                    if (stepStatus.Any())
                    {
                        await _dbContext.StepStatus.AddRangeAsync(stepStatus);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.StructTypes.Any())
                {
                    var structTypes = GetStructTypeFromFile();
                    if (structTypes.Any())
                    {
                        await _dbContext.StructTypes.AddRangeAsync(structTypes);
                        _dbContext.SaveChanges();
                    }
                }

                // Job -> Steps -> Operations
                // Issue: Duplicate id because before inserted

                if (!_dbContext.Jobs.Any())
                {
                    var jobs = GetJobFromFile();
                    if (jobs.Any())
                    {
                        await _dbContext.Jobs.AddRangeAsync(jobs);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.Steps.Any())
                {
                    var steps = GetStepFromFile();
                    if (steps.Any())
                    {
                        await _dbContext.Steps.AddRangeAsync(steps);
                        _dbContext.SaveChanges();
                    }
                }

                if (!_dbContext.Operations.Any())
                {
                    var operations = GetOperationFromFile();
                    if (operations.Any())
                    {
                        await _dbContext.Operations.AddRangeAsync(operations);
                        _dbContext.SaveChanges();
                    }
                }

            });
        }

        private IEnumerable<JobStatus> GetJobStatusFromFile()
        {
            string csvFile = GetPathToFile("JobStatus.csv");
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<JobStatus>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => CreateJobStatus(x, ref id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private JobStatus CreateJobStatus(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("JobStatus is null or empty");
            }

            return new JobStatus(id++, value.Trim('"').Trim().ToLowerInvariant());
        }


        private IEnumerable<StepStatus> GetStepStatusFromFile()
        {
            string csvFile = GetPathToFile("StepStatus.csv");
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<StepStatus>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => CreateStepStatus(x, ref id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private StepStatus CreateStepStatus(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("JobStepStatus is null or empty");
            }

            return new StepStatus(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private IEnumerable<StructType> GetStructTypeFromFile()
        {
            string csvFile = GetPathToFile("StructTypes.csv");
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<StructType>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => CreateStructType(x, ref id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private StructType CreateStructType(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("StructType is null or empty");
            }

            return new StructType(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private IEnumerable<Job> GetJobFromFile()
        {
            string csvFile = GetPathToFile("Jobs.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Job>();
            }

            string[] requiredHeaders = { "ProductId", "Description", "JobStatus" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateJob(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }


        private Job CreateJob(string[] column, string[] headers)
        {
            var job = new Job()
            {
                ProductId = int.Parse(column[Array.IndexOf(headers, "ProductId".ToLower())].Trim('"').Trim()),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                JobStatus = Enumeration.FromDisplayName<JobStatus>(CsvExtensions.GetColumnValueIgnoreCase(column, headers, "JobStatus")),
                DateCreated = DateTime.UtcNow,
            };

            return job;
        }

        private IEnumerable<Step> GetStepFromFile()
        {
            string csvFile = GetPathToFile("Steps.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Step>();
            }

            string[] requiredHeaders = { "SpecificationId", "WorkerId", "SkillId", "OrderNumber", "StepStatus" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateStep(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }


        private Step CreateStep(string[] column, string[] headers)
        {
            var step = new Step()
            {
                SpecificationId = int.Parse(column[Array.IndexOf(headers, "SpecificationId".ToLower())].Trim('"').Trim()),
                WorkerId = int.Parse(column[Array.IndexOf(headers, "WorkerId".ToLower())].Trim('"').Trim()),
                SkillId = int.Parse(column[Array.IndexOf(headers, "SkillId".ToLower())].Trim('"').Trim()),
                OrderNumber = int.Parse(column[Array.IndexOf(headers, "OrderNumber".ToLower())].Trim('"').Trim()),
                StepStatus = Enumeration.FromDisplayName<StepStatus>(CsvExtensions.GetColumnValueIgnoreCase(column, headers, "StepStatus")),
                DateCreated = DateTime.UtcNow,
            };

            return step;
        }

        private IEnumerable<Operation> GetOperationFromFile()
        {
            string csvFile = GetPathToFile("Operations.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Operation>();
            }

            string[] requiredHeaders = { "StructTypeId", "IsAsync", "OrderNumber", "JobId", "StepIds" };
            string[] headers = csvFile.GetHeaders(requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateOperation(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }


        private Operation CreateOperation(string[] column, string[] headers)
        {
            var stepIds = column[Array.IndexOf(headers, "StepIds".ToLower())].Trim('"').Trim().Split(",").Select(int.Parse).ToList();
            var steps = _dbContext.Steps.Where(s => stepIds.Contains(s.Id)).ToList();

            var operation = new Operation()
            {
                StructTypeId = int.Parse(column[Array.IndexOf(headers, "StructTypeId".ToLower())].Trim('"').Trim()),
                IsAsync = bool.Parse(column[Array.IndexOf(headers, "IsAsync".ToLower())].Trim('"').Trim()),
                OrderNumber = int.Parse(column[Array.IndexOf(headers, "OrderNumber".ToLower())].Trim('"').Trim()),
                JobId = int.Parse(column[Array.IndexOf(headers, "JobId".ToLower())].Trim('"').Trim()),
                Steps = steps,
                DateCreated = DateTime.UtcNow,
            };

            return operation;
        }

    }
}
