using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dorywcza.Data;
using Dorywcza.Models;
using Microsoft.AspNetCore.Cors;

namespace Dorywcza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                var employees = _context.Employees.ToList();
                return Ok(employees);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Employees/1
        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(a => a.EmployeeId == id);

                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Employees/1
        [HttpPut("{id}")]
        public IActionResult PutEmployee(int id, [FromBody]Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != employee.EmployeeId) return BadRequest();

            try
            {
                if ((_context.Employees.FirstOrDefault(a => a.EmployeeId == id)) == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Entry(employee).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok("Data updated");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Employees
        [HttpPost]
        public IActionResult PostEmployee([FromBody]Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return Ok("Data created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Employees/1
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(a => a.EmployeeId == id);
                if (employee == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChanges();
                    return Ok("Data deleted");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
