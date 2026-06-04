using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models; // 👈 Make sure this matches where your 'Student' model lives

namespace WebApiDemo.Data
{
    public class StudentDbContext : DbContext
    {
        // This constructor passes your settings down to the Entity Framework Core engine
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
        }

        // This defines your table map
        public DbSet<Student> Students { get; set; }
    }
}