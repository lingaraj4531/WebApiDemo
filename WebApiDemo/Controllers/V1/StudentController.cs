using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Models;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _service.GetAllAsync();

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _service.GetByIdAsync(id);

            if (student == null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            var result = await _service.CreateAsync(student);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,Student student)
        {
            student.Studentid = id;

            await _service.UpdateAsync(student);

            return NoContent();
        }

        /// <summary>
       

        /// <summary>
        /// V2 Delete Route: Soft Delete Business Logic
        /// URL Path: DELETE /api/v2/Student/{id}
        ///// </summary>
        //[HttpDelete("{id}")]
        //[MapToApiVersion("2.0")]
        //public async Task<IActionResult> DeleteV2(int id)
        //{
        //    var success = await _service.SoftDeleteAsync(id);
        //    if (!success) return NotFound();

        //    return Ok(new { message = "Student successfully archived via production soft-delete rules." });
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
