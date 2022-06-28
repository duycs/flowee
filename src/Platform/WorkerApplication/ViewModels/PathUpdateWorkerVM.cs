using System.ComponentModel.DataAnnotations;

namespace WorkerApplication.ViewModels
{
    public class PathUpdateWorkerVM
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public int[]? RoleIds { get; set; }
        public int[]? GroupIds { get; set; }
        public int[]? SkillIds { get; set; }
    }
}
