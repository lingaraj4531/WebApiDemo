using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Models;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _service.GetAllAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _service.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            var result = await _service.CreateAsync(employee);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,Employee employee)
        {
            employee.Employeeid = id;

            await _service.UpdateAsync(employee);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
