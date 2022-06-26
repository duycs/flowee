namespace WorkerApplication.ViewModels
{
    public class UpdateWorkerVM
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public List<int>? RoleIds { get; set; }
        public List<int>? GroupIds { get; set; }
    }
}
