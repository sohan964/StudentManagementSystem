using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class StudentDBContext : IdentityDbContext<ApplicationUser> 
    {
        public StudentDBContext(DbContextOptions<StudentDBContext> options): base(options)
        {
            
        }

    }
}
