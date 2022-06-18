﻿using AppShareServices.Models;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WorkerDomain.AgreegateModels.TimeKeepingAgreegate;
using WorkerDomain.AgreegateModels.WorkerAgreegate;
using WorkerInfrastructure.DataAccess;
using Group = WorkerDomain.AgreegateModels.WorkerAgreegate.Group;

namespace WorkerAPI.SeedData
{
    /// <summary>
    /// Seed master data tables: departments, roles, shifts, skills, skillLevels
    /// Seed Relations tables: groups, workers, workerGroups, workerSkills, timeKeepings
    /// </summary>
    public class WorkerContextSeed
    {
        private WorkerContext _context;
        private IConfiguration _configuration;
        private ILogger<WorkerContextSeed> _logger;
        private IWebHostEnvironment _hostEnvironment;
        private string ContentRootPath;
        private const string SeedDataFolder = "SeedData";

        public WorkerContextSeed(WorkerContext context, ILogger<WorkerContextSeed> logger,
            IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            ContentRootPath = hostEnvironment.ContentRootPath;
        }

        public void Migrate()
        {
            _context.Database.Migrate();
        }

        // Seed to database
        public async Task SeedAsync()
        {
            var policy = CreatePolicy(_logger, nameof(WorkerContextSeed));

            await policy.ExecuteAsync(async () =>
            {

                // Master data
                if (!_context.Roles.Any())
                {
                    var roles = GetRolesFromFile();
                    if (roles.Any())
                    {
                        await _context.Roles.AddRangeAsync(roles);
                        _context.SaveChanges();
                    }
                }

                if (!_context.Departments.Any())
                {
                    var departments = GetDepartmentFromFile();
                    if (departments.Any())
                    {
                        await _context.Departments.AddRangeAsync(departments);
                        _context.SaveChanges();
                    }
                }

                if (!_context.Shifts.Any())
                {
                    var shifts = GetShiftFromFile();
                    if (shifts.Any())
                    {
                        await _context.Shifts.AddRangeAsync(shifts);
                        _context.SaveChanges();
                    }
                }

                if (!_context.Skills.Any())
                {
                    var skills = GetSkillFromFile();
                    await _context.Skills.AddRangeAsync(skills);
                    _context.SaveChanges();
                }

                if (!_context.SkillLevels.Any())
                {
                    var skillLevels = GetSkillLevelFromFile();
                    await _context.SkillLevels.AddRangeAsync(skillLevels);
                    _context.SaveChanges();
                }

                // Relations data
                if (!_context.Groups.Any())
                {
                    var groups = GetGroupFromFile();
                    if (groups.Any())
                    {
                        await _context.Groups.AddRangeAsync(groups);
                        _context.SaveChanges();
                    }
                }

                if (!_context.Workers.Any())
                {
                    var workers = GetWorkersFromFile();
                    if (workers.Any())
                    {
                        await _context.Workers.AddRangeAsync(workers);
                        _context.SaveChanges();
                    }
                }

                if (!_context.WorkerGroups.Any())
                {
                    var workerGroups = GetWorkerGroupFromFile();
                    if (workerGroups.Any())
                    {
                        await _context.WorkerGroups.AddRangeAsync(workerGroups);
                        _context.SaveChanges();
                    }
                }

                if (!_context.WorkerSkills.Any())
                {
                    var workerSkills = GetWorkerSkillFromFile();
                    if (workerSkills.Any())
                    {
                        await _context.WorkerSkills.AddRangeAsync(workerSkills);
                        _context.SaveChanges();
                    }
                }

                if (!_context.TimeKeepings.Any())
                {
                    var timeKeepings = GetTimeKeepingFromFile();
                    if (timeKeepings.Any())
                    {
                        await _context.TimeKeepings.AddRangeAsync(timeKeepings);
                        _context.SaveChanges();
                    }
                }

                await _context.SaveChangesAsync();
            });
        }

        // Get from files
        private IEnumerable<Department> GetDepartmentFromFile()
        {
            string csvFile = GetPathToFile("Departments.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Department>();
            }

            string[] requiredHeaders = { "Code", "Name", "Description" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateDepartment(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }
        private IEnumerable<Group> GetGroupFromFile()
        {
            string csvFile = GetPathToFile("Groups.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Group>();
            }

            string[] requiredHeaders = { "Code", "Name", "Description", "DepartmentCode" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateGroup(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }
        private IEnumerable<Role> GetRolesFromFile()
        {
            string csvFile = GetPathToFile("Roles.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Role>();
            }

            string[] requiredHeaders = { "Code", "Name", "Description" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateRole(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }
        private IEnumerable<Shift> GetShiftFromFile()
        {
            string csvFile = GetPathToFile("Shifts.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Shift>();
            }

            string[] requiredHeaders = { "Code", "Name", "TimeStart", "TimeEnd", "IsNormal" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateShift(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }
        private IEnumerable<SkillLevel> GetSkillLevelFromFile()
        {
            string csvFile = GetPathToFile("SkillLevels.csv");
            if (!File.Exists(csvFile))
            {
                return Enumeration.GetAll<SkillLevel>();
            }

            int id = 1;
            return File.ReadAllLines(csvFile)
                                        .SelectTry(x => CreateSkillLevel(x, ref id))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }
        private IEnumerable<Skill> GetSkillFromFile()
        {
            string csvFile = GetPathToFile("Skills.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Skill>();
            }

            string[] requiredHeaders = { "Code", "Name", "Description" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateSkill(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }
        private IEnumerable<TimeKeeping> GetTimeKeepingFromFile()
        {
            string csvFile = GetPathToFile("TimeKeepings.csv");

            if (!File.Exists(csvFile))
            {
                return new List<TimeKeeping>();
            }

            string[] requiredHeaders = { "WorkerCode", "ShiftCode" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateTimeKeeping(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }
        private IEnumerable<WorkerGroup> GetWorkerGroupFromFile()
        {
            string csvFile = GetPathToFile("WorkerGroups.csv");

            if (!File.Exists(csvFile))
            {
                return new List<WorkerGroup>();
            }

            string[] requiredHeaders = { "WorkerCode", "GroupCode", "IsActive" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateWorkerGroup(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }
        private IEnumerable<Worker> GetWorkersFromFile()
        {
            string csvFile = GetPathToFile("Workers.csv");

            if (!File.Exists(csvFile))
            {
                return new List<Worker>();
            }

            string[] requiredHeaders = { "Email", "Code", "FullName", "RoleCode" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateWorker(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }
        private IEnumerable<WorkerSkill> GetWorkerSkillFromFile()
        {
            string csvFile = GetPathToFile("WorkerSkills.csv");

            if (!File.Exists(csvFile))
            {
                return new List<WorkerSkill>();
            }

            string[] requiredHeaders = { "WorkerCode", "SkillCode", "SkillLevelName", "IsActive", "IsPriority" };
            string[] headers = GetHeaders(csvFile, requiredHeaders);

            return File.ReadAllLines(csvFile)
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateWorkerSkill(column, headers))
                                        .OnCaughtException(ex => { _logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        // Create entity
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
            var departmentCode = column[Array.IndexOf(headers, "DepartmentCode".ToLower())].Trim('"').Trim();
            var department = _context.Departments.FirstOrDefault(i => i.Code.ToLower() == departmentCode);
            if (department == null)
            {
                throw new Exception($"Department {departmentCode} not found");
            }

            var group = new Group()
            {
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                DepartmentId = department.Id,
                Description = column[Array.IndexOf(headers, "Description".ToLower())].Trim('"').Trim(),
                DateCreated = DateTime.UtcNow,
            };

            return group;
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
            var roleCode = column[Array.IndexOf(headers, "RoleCode".ToLower())].Trim('"').Trim();
            var role = _context.Roles.FirstOrDefault(i => i.Code.ToLower() == roleCode);

            if (role == null)
            {
                throw new Exception($"Role {roleCode} not found");
            }

            var worker = new Worker()
            {
                FullName = column[Array.IndexOf(headers, "FullName".ToLower())].Trim('"').Trim(),
                Email = column[Array.IndexOf(headers, "Email".ToLower())].Trim('"').Trim(),
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                RoleId = role.Id,
                DateCreated = DateTime.UtcNow,
            };

            return worker;
        }

        private Shift CreateShift(string[] column, string[] headers)
        {
            var shift = new Shift()
            {
                Code = column[Array.IndexOf(headers, "Code".ToLower())].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "Name".ToLower())].Trim('"').Trim(),
                TimeStart = TimeOnly.Parse(column[Array.IndexOf(headers, "TimeStart".ToLower())].Trim('"').Trim()),
                TimeEnd = TimeOnly.Parse(column[Array.IndexOf(headers, "TimeEnd".ToLower())].Trim('"').Trim()),
                IsNormal = bool.Parse(column[Array.IndexOf(headers, "IsNormal".ToLower())].Trim('"').Trim()),
                DateCreated = DateTime.UtcNow,
            };

            return shift;
        }

        private SkillLevel CreateSkillLevel(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("SkillLevel is null or empty");
            }

            return new SkillLevel(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private TimeKeeping CreateTimeKeeping(string[] column, string[] headers)
        {
            var workerCode = column[Array.IndexOf(headers, "WorkerCode".ToLower())].Trim('"').Trim();
            var worker = _context.Workers.FirstOrDefault(i => i.Code.ToLower() == workerCode);
            var shiftCode = column[Array.IndexOf(headers, "ShiftCode".ToLower())].Trim('"').Trim();
            var shift = _context.Shifts.FirstOrDefault(i => i.Code.ToLower() == shiftCode);

            if (worker == null)
            {
                throw new Exception($"Worker {workerCode} not found");
            }

            if (shift == null)
            {
                throw new Exception($"Shift {shiftCode} not found");
            }

            var timeKeeping = new TimeKeeping()
            {
                WorkerId = worker.Id,
                ShiftId = shift.Id,
                DateCreated = DateTime.UtcNow,
            };

            return timeKeeping;
        }

        private WorkerGroup CreateWorkerGroup(string[] column, string[] headers)
        {
            var workerCode = column[Array.IndexOf(headers, "WorkerCode".ToLower())].Trim('"').Trim();
            var worker = _context.Workers.FirstOrDefault(i => i.Code.ToLower() == workerCode);
            var groupCode = column[Array.IndexOf(headers, "GroupCode".ToLower())].Trim('"').Trim();
            var group = _context.Groups.FirstOrDefault(i => i.Code.ToLower() == groupCode);

            if (worker == null)
            {
                throw new Exception($"Worker {workerCode} not found");
            }

            if (group == null)
            {
                throw new Exception($"Group {groupCode} not found");
            }

            var workerGroup = new WorkerGroup()
            {
                WorkerId = worker.Id,
                GroupId = group.Id,
                IsActive = bool.Parse(column[Array.IndexOf(headers, "IsActive".ToLower())].Trim('"').Trim()),
                DateCreated = DateTime.UtcNow,
            };

            return workerGroup;
        }

        private WorkerSkill CreateWorkerSkill(string[] column, string[] headers)
        {
            var workerCode = column[Array.IndexOf(headers, "WorkerCode".ToLower())].Trim('"').Trim();
            var worker = _context.Workers.FirstOrDefault(i => i.Code.ToLower() == workerCode);
            var skillCode = column[Array.IndexOf(headers, "SkillCode".ToLower())].Trim('"').Trim();
            var skill = _context.Skills.FirstOrDefault(i => i.Code.ToLower() == skillCode);
            var skillLevelName = column[Array.IndexOf(headers, "SkillLevelName".ToLower())].Trim('"').Trim();
            var skillLevel = _context.SkillLevels.FirstOrDefault(i => i.Name.ToLower() == skillLevelName);

            if (worker == null)
            {
                throw new Exception($"Worker {workerCode} not found");
            }

            if (skill == null)
            {
                throw new Exception($"Skill {skillCode} not found");
            }

            if (skillLevel == null)
            {
                throw new Exception($"SkillLevel {skillLevel} not found");
            }

            var workerSkill = new WorkerSkill()
            {
                WorkerId = worker.Id,
                SkillId = skill.Id,
                SkillLevelId = skillLevel.Id,
                IsActive = bool.Parse(column[Array.IndexOf(headers, "IsActive".ToLower())].Trim('"').Trim()),
                IsPriority = bool.Parse(column[Array.IndexOf(headers, "IsPriority".ToLower())].Trim('"').Trim()),
                DateCreated = DateTime.UtcNow,
            };

            return workerSkill;
        }

        // CSV helper

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

        private AsyncRetryPolicy CreatePolicy(ILogger<WorkerContextSeed> _logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        _logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }

        private string GetPathToFile(string csvFile)
        {
            return Path.Combine(ContentRootPath, SeedDataFolder, csvFile);
        }

        private string GetPathToFile(string folder, string csvFile)
        {
            return Path.Combine(ContentRootPath, folder, "Departments.csv");
        }
    }
}
