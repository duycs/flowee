using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WorkerDomain.AgreegateModels.WorkerAgreegate;
using Group = WorkerDomain.AgreegateModels.WorkerAgreegate.Group;

namespace WorkerInfrastructure.DataAccess
{
    public class WorkerContextSeed
    {
        public async Task SeedAsync(WorkerContext context, IWebHostEnvironment env, IConfiguration configuration, ILogger<WorkerContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(WorkerContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                var contentRootPath = env.ContentRootPath;

                if (!context.Roles.Any())
                {
                    var roles = GetRolesFromFile(contentRootPath, logger);
                    if (roles.Any())
                    {
                        await context.Roles.AddRangeAsync(roles);
                    }
                }

                if (!context.Groups.Any())
                {
                    var groups = GetGroupFromFile(contentRootPath, logger);
                    if (groups.Any())
                    {
                        await context.Groups.AddRangeAsync(groups);
                    }
                }

                if (!context.Departments.Any())
                {
                    var departments = GetDepartmentFromFile(contentRootPath, logger);
                    if (departments.Any())
                    {
                        await context.Departments.AddRangeAsync(departments);
                    }
                }

                if (!context.Workers.Any())
                {
                    var workers = GetWorkersFromFile(contentRootPath, logger);
                    if (workers.Any())
                    {
                        await context.Workers.AddRangeAsync(workers);
                    }
                }

                await context.SaveChangesAsync();
            });
        }

        private IEnumerable<Worker> GetWorkersFromFile(string contentRootPath, ILogger<WorkerContextSeed> logger)
        {
            string csvFile = Path.Combine(contentRootPath, "SeedData", "Workers.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Worker>();
            }

            string[] requiredHeaders = { "FullName", "Email", "Role", "Department" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateWorker(column, headers))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private IEnumerable<Role> GetRolesFromFile(string contentRootPath, ILogger<WorkerContextSeed> logger)
        {
            string csvFile = Path.Combine(contentRootPath, "SeedData", "Roles.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Role>();
            }

            string[] requiredHeaders = { "Name", "Code", "Description" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateRole(column, headers))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private IEnumerable<Group> GetGroupFromFile(string contentRootPath, ILogger<WorkerContextSeed> logger)
        {
            string csvFile = Path.Combine(contentRootPath, "SeedData", "Roles.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Group>();
            }

            string[] requiredHeaders = { "Name", "Code", "Description" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateGroup(column, headers))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private IEnumerable<Department> GetDepartmentFromFile(string contentRootPath, ILogger<WorkerContextSeed> logger)
        {
            string csvFile = Path.Combine(contentRootPath, "SeedData", "Departments.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Department>();
            }

            string[] requiredHeaders = { "Name", "Code", "Description" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateDepartment(column, headers))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private Role CreateRole(string[] column, string[] headers)
        {
            var role = new Role()
            {
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return role;
        }

        private Group CreateGroup(string[] column, string[] headers)
        {
            var group = new Group()
            {
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return group;
        }

        private Department CreateDepartment(string[] column, string[] headers)
        {
            var department = new Department()
            {
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return department;
        }

        private Worker CreateWorker(string[] column, string[] headers)
        {
            string email = column[Array.IndexOf(headers, "Email".ToLower())].Trim('"').Trim();
            string code = email.Split("@")[0];
            var worker = new Worker()
            {
                FullName = column[Array.IndexOf(headers, "FullName".ToLower())].Trim('"').Trim(),
                Email = email,
                Code = code,
                DateCreated = DateTime.UtcNow,
            };

            return worker;
        }

        private string[] GetHeaders(string csvfile, string[] requiredHeaders, string[] optionalHeaders = null)
        {
            string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

            if (csvheaders.Count() < requiredHeaders.Count())
            {
                throw new Exception($"requiredHeader count '{requiredHeaders.Count()}' is bigger then csv header count '{csvheaders.Count()}' ");
            }

            if (optionalHeaders != null)
            {
                if (csvheaders.Count() > (requiredHeaders.Count() + optionalHeaders.Count()))
                {
                    throw new Exception($"csv header count '{csvheaders.Count()}'  is larger then required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
                }
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains(requiredHeader.ToLower()))
                {
                    throw new Exception($"does not contain required header '{requiredHeader}'");
                }
            }

            return csvheaders;
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<WorkerContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
