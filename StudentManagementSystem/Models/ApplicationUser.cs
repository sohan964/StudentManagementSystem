using Microsoft.AspNetCore.Identity;

namespace StudentManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
