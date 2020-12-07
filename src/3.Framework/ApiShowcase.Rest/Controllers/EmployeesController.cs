using System.Linq;
using System.Threading.Tasks;
using ApiShowcase.Drivers.Data.SQLServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiShowcase.Rest.Controllers
{
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly AdventureWorksContext _context;

        public EmployeesController(AdventureWorksContext context)
        {
            _context = context;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employees = await _context.Employees.ToListAsync();
            Response.Headers.Add("X-Total-Count", employees.Count().ToString());
            return Ok(employees);
        }

        //// GET api/employees/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    if (id <= 0) return BadRequest(new { message = "This is an invalid ID." });
        //    var employee = await repository.Get(id);
        //    if (employee == null) return NotFound(new { message = "Employee not found."});
        //    return Ok(employee);
        //}

        //// POST api/employees
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] EmployeeRecordRequest data)
        //{
        //    if (data == null) return BadRequest();
        //    if (!ModelState.IsValid) return BadRequest();
        //    var employee = await repository.Create(data);
        //    var url = $"{HttpContext.Request.Path}/{employee.Id}";
        //    return Created(url, employee);
        //}

        //// PUT api/employees/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, [FromBody] EmployeeRecordRequest data)
        //{
        //    if (id <= 0) return BadRequest(new { message = "This is an invalid ID." });
        //    if (data == null) return BadRequest();
        //    var exists = await repository.Exists(id);
        //    if (!exists) return NotFound();
        //    if (!ModelState.IsValid) return BadRequest();
        //    await repository.Update(data);
        //    return NoContent();
        //}

        //// DELETE api/employees/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (id <= 0) return BadRequest(new { message = "This is an invalid ID." });
        //    var employee = await repository.Get(id);
        //    if (employee == null) return NotFound(new { message = "Employee not found." });
        //    await repository.Delete(employee);
        //    return NoContent();
        //}
    }
}
