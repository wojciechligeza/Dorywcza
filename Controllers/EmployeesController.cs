using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dorywcza.Data;
using Dorywcza.Models;
using Dorywcza.Services.EmailService;
using Microsoft.AspNetCore.Cors;

namespace Dorywcza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailProvider _emailProvider;

        public EmployeesController(ApplicationDbContext context, IEmailProvider emailProvider)
        {
            _context = context;
            _emailProvider = emailProvider;
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
                EmailSend(employee);
                return Ok("Data created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region HttpPostEmployee Helper
        private void EmailSend(Employee employee)
        {
            var emailAddress = new EmailAddress(employee.Name, employee.Email);
            var emailMessage = new EmailMessage();

            try
            {
                emailMessage.FromAddresses.Add(new EmailAddress());
                emailMessage.ToAddresses.Add(emailAddress);
                emailMessage.Subject = "Prośba o pracę";

                using (var fileStream = new FileStream(@"Services\EmailService\EmailToEmployee.txt", FileMode.Open, FileAccess.Read))
                {
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        emailMessage.Content = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionAlert(e.Message);
            }

            _emailProvider.Send(emailMessage);
        }

        public IActionResult ExceptionAlert(string e)
        {
            if(e!=null) return BadRequest(e);
            else return NotFound();
        }
        #endregion
        
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
