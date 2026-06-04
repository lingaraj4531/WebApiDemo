using WebApiDemo.Models;

namespace WebApiDemo.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();

        Task<Student> GetByIdAsync(int id);

        Task<Student> CreateAsync(Student student);

        Task UpdateAsync(Student student);

        Task DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
        //Task HardDeleteAsync(int id);
    }
}
