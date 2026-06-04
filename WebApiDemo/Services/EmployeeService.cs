

using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;
using WebApiDemo.Models;

namespace WebApiDemo.Services
{
    public class EmployeeService:IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);

                await _context.SaveChangesAsync();
            }
        }
    }
}
