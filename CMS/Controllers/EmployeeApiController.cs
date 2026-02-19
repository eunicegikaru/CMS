// Controllers/EmployeeApiController.cs
using DBL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly IEmployeesRepository _repo;

        public EmployeeApiController(IEmployeesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _repo.GetAllEmployees();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var emp = await _repo.GetEmployeeById(id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] DBL.Models.Employee employee)
        {
            var id = await _repo.CreateEmployee(employee);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DBL.Models.Employee employee)
        {
            employee.Id = id;
            var updated = await _repo.UpdateEmployee(employee);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteEmployee(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/activity")]
        public async Task<IActionResult> Activity(int id)
        {
            var activities = await _repo.GetEmployeeActivity(id);
            return Ok(activities);
        }
    }
}
