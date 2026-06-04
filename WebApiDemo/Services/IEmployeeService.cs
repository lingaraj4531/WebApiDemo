using WebApiDemo.Models;

namespace WebApiDemo.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee> GetByIdAsync(int id);

        Task<Employee> CreateAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(int id);
    }
}
