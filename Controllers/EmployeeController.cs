using empcoreapiproj.Data;
using empcoreapiproj.Services;
using Microsoft.AspNetCore.Mvc;
using empcoreapiproj.ViewModel;
using Microsoft.AspNetCore.Authorization;
using empcoreapiproj.Models;

namespace empcoreapiproj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class EmployeeController : ControllerBase
    {
        public EmployeeService service;
        public ApplicationDbContext context;
        public EmployeeController(EmployeeService serve, ApplicationDbContext contex)
        {
            service = serve;
            context = contex;
        }
        [Authorize]
        [HttpGet("get-all-employees")]

        public IActionResult GetAllEmployees()
        {
            var vf = service.GetEmployee();
            return Ok(vf);
        }
        [Authorize]
        [HttpGet("get-employee-by-id/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var cd = service.GetEmployeeById(id);
            return Ok(cd);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("add-employee")]
        public IActionResult AddEmployee([FromBody] EmployeeVm vm)
        {
            service.AddEmployee(vm);
            return Ok();
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("update-employee-by-id/{id}")]
        public IActionResult UpdateEmployeebyId(int id, [FromBody] EmployeeVm vm)
        {
            var update = service.UpdateEmployeeById(id, vm);
            return Ok(update);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("delete-employee-by-id/{id}")]
        public IActionResult DeleteEmployeebyId(int id)
        {
            service.DeleteEmployeebyId(id);
            return Ok();
        }
    }
}