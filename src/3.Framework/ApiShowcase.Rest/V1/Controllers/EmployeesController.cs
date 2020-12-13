using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiShowcase.Drivers.Data.SQLServer.Models;
using ApiShowcase.Rest.Models;
using ApiShowcase.Rest.V1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiShowcase.Rest.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly AdventureWorksContext _context;

        public EmployeesController(AdventureWorksContext context)
        {
            _context = context;
        }

        // GET: api/employees
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery]GetRequestModel parameters)
        {
            var employeesQuery = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(parameters._order))
            {
                switch (parameters._order.ToUpperInvariant())
                {
                    case "BIRTHDATE":
                    case "BIRTHDATE ASC": employeesQuery = employeesQuery.OrderBy(o => o.BirthDate); break;
                    case "BIRTHDATE DESC": employeesQuery = employeesQuery.OrderByDescending(o => o.BirthDate); break;
                    case "HIREDATE":
                    case "HIREDATE ASC": employeesQuery = employeesQuery.OrderBy(o => o.HireDate); break;
                    case "HIREDATE DESC": employeesQuery = employeesQuery.OrderByDescending(o => o.HireDate); break;
                    default: return BadRequest(new ErrorResponse{ Message = "Cannot sort by the given property" });
                }
            }
            else employeesQuery = employeesQuery.OrderBy(o => o.BusinessEntityId);

            var employees = await employeesQuery
                .Skip(parameters._offset.GetValueOrDefault(0))
                .Take(parameters._limit.GetValueOrDefault(10))
                .ToListAsync();
            var employeesTotalCount = await employeesQuery.CountAsync();
            Response.Headers.Add("X-Total-Count", employeesTotalCount.ToString());
            return Ok(employees);
        }

        // GET api/employees/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Employee), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest(new ErrorResponse { Message = "This is an invalid ID." });
            var employee = await _context.Employees.FirstOrDefaultAsync(w => w.BusinessEntityId == id);
            if (employee == null) return NotFound(new ErrorResponse { Message = "Employee not found."});
            return Ok(employee);
        }

        // POST api/employees
        [HttpPost]
        [ProducesResponseType(typeof(Employee), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Employee data)
        {
            if (data == null) return BadRequest(new ErrorResponse { Message = "Employee data cannot be null."});
            if (!ModelState.IsValid) return BadRequest(new ErrorResponse { Message = "Employee data has invalid data." });
            var newData = await _context.Employees.AddAsync(data);
            await _context.SaveChangesAsync();
            var newDataEntity = newData.Entity;
            var url = $"{HttpContext.Request.Path}/{newDataEntity.BusinessEntity}";
            return Created(url, newDataEntity);
        }

        // PUT api/employees/5
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] Employee data)
        {
            if (id <= 0) return BadRequest(new ErrorResponse { Message = "This is an invalid ID." });
            if (data == null) return BadRequest(new ErrorResponse { Message = "Employee data cannot be null." });
            if (!ModelState.IsValid) return BadRequest(new { message = "Employee data has invalid data." });
            var exists = (await _context.Employees.FirstOrDefaultAsync(w => w.BusinessEntityId == id)) != null;
            if (!exists) return NotFound(new ErrorResponse { Message = "Employee cannot be found." });
            _context.Employees.Update(data);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/employees/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest(new ErrorResponse { Message = "This is an invalid ID." });
            var data = await _context.Employees.FirstOrDefaultAsync(w => w.BusinessEntityId == id);
            var exists = data != null;
            if (!exists) return NotFound(new ErrorResponse { Message = "Employee cannot be found." });
            _context.Employees.Remove(data);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
