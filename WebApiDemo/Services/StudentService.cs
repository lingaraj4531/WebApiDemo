

using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;
using WebApiDemo.Models;

namespace WebApiDemo.Services
{
    public class StudentService:IStudentService
    {
        private readonly StudentDbContext _context;

        public StudentService(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> CreateAsync(Student student)
        {
            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);

            await _context.SaveChangesAsync();
        }

        // 💥 V1 Rule: Permanent Wipe
        //public async Task HardDeleteAsync(int id)
        //{
        //    var student = await _context.Students.FindAsync(id);
        //    if (student != null)
        //    {
        //        _context.Students.Remove(student);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        // 🛡️ V2 Rule: Safe Archive (Soft Delete)
        public async Task<bool> SoftDeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            // If student doesn't exist or is already soft-deleted, exit early
            if (student == null || student.IsDeleted) return false;

            // CRITICAL BUSINESS LOGIC: Change data state instead of removing
            student.IsDeleted = true;
            student.DeletedTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                _context.Students.Remove(student);

                await _context.SaveChangesAsync();
            }
        }
    }
}
