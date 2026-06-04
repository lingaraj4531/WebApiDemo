using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Models;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteV2(int id)
        {
            var success = await _service.SoftDeleteAsync(id);
            if (!success) return NotFound();

            return Ok(new { message = "Student successfully archived via production soft-delete rules." });
        }
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _service.DeleteAsync(id);

        //    return NoContent();
        //}
    }
}
