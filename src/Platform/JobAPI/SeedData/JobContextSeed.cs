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


        private IEnumerable<StepStatus> GetJobStepStatusFromFile()
        {
            string csvFile = GetPathToFile("JobStepStatus.csv");
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<StepStatus>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => CreateJobStepStatus(x, ref id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private StepStatus CreateJobStepStatus(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("JobStepStatus is null or empty");
            }

            return new StepStatus(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private IEnumerable<Job> GetJobFromFile()
        {
            string csvFile = GetPathToFile("Jobs.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Job>();
            }

            string[] requiredHeaders = { "ProductId" };
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

            string[] requiredHeaders = { "WorkerId", "SkillId", "ProductId", "JobStepStatus", "JobId" };
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
                WorkerId = int.Parse(column[Array.IndexOf(headers, "WorkerId".ToLower())].Trim('"').Trim()),
                SkillId = int.Parse(column[Array.IndexOf(headers, "SkillId".ToLower())].Trim('"').Trim()),
                DateCreated = DateTime.UtcNow,
            };

            return step;
        }

    }
}
