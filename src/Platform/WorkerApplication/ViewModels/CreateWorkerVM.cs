using System.ComponentModel.DataAnnotations;

namespace WorkerApplication.ViewModels
{
    public class CreateWorkerVM
    {
        [Required]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string? Code { get; set; }
        public int? RoleId { get; set; }
        public int? GroupId { get; set; }
    }
}
