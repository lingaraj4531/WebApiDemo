using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models;


namespace WebApiDemo.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        
    }
}
