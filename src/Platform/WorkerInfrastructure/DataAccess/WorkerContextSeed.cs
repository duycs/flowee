using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

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

        private Worker CreateWorker(string[] column, string[] headers)
        {
            var worker = new Worker()
            {
                FullName = column[Array.IndexOf(headers, "FullName".ToLower())].Trim('"').Trim(),
                Email = column[Array.IndexOf(headers, "Email".ToLower())].Trim('"').Trim(),
                Role = (Role)int.Parse(column[Array.IndexOf(headers, "Role".ToLower())].Trim('"').Trim()),
                Department = (Department)int.Parse(column[Array.IndexOf(headers, "Department".ToLower())].Trim('"').Trim()),
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
