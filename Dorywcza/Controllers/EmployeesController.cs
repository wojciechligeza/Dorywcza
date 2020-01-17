using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dorywcza.Data;
using Dorywcza.Models;
using Dorywcza.Services.EmailService;
using Dorywcza.Services.EmailService.Helpers;
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
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees
                    .Include(a => a.JobOfferEmployees)
                    .ThenInclude(b => b.JobOffer).ToListAsync();

                return Ok(employees);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Employees/forEmployer/1
        [HttpGet("forEmployer/{id}")]
        public async Task<IActionResult> GetEmployeesForEmployer(int id)
        {
            try
            {
                var employees = await _context.Employees
                    .Include(a => a.JobOfferEmployees)
                    .ThenInclude(b => b.JobOffer).ToListAsync();

                var employer = await _context.Employers.FirstOrDefaultAsync(a => a.UserId == id);

                if (employer != null)
                {
                    if (employer.JobOffers.Count == 1)
                    {
                        var result = from employee in employees
                                            let jobOffers = employee.JobOfferEmployees
                                            .Where(a => a.JobOffer.EmployerId == employer.EmployerId)
                                    where jobOffers.Any()
                                    select employee;

                        return Ok(result);
                    }
                    else
                    {

                        var results = employees.SelectMany(a =>
                            a.JobOfferEmployees.Where(b => b.JobOffer.EmployerId == employer.EmployerId));

                        return Ok(results);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Employees/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(a => a.JobOfferEmployees)
                    .ThenInclude(b => b.JobOffer).FirstOrDefaultAsync(a => a.EmployeeId == id);

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
        public async Task<IActionResult> PutEmployee(int id, [FromBody]Employee employee)
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
                    await _context.SaveChangesAsync();
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
                SendFirstEmail(employee);
                return Ok("Data created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region HttpPostEmployee Helper
        private void SendFirstEmail(Employee employee)
        {
            var emailAddress = new EmailAddress(employee.Name, employee.Email);
            var emailMessage = new EmailMessage();

            try
            {
                emailMessage.FromAddresses.Add(new EmailAddress());
                emailMessage.ToAddresses.Add(emailAddress);
                emailMessage.Subject = "Praca";

                string filePath = @"Services\EmailService\EmailText\EmailToEmployee.txt";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8);

                emailMessage.Content = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                ExceptionAlert(e.Message);
            }

            _emailProvider.Send(emailMessage);
        }
        #endregion
        
        // DELETE: api/Employees/1/yes
        [HttpDelete("{id}/{status}")]
        public IActionResult DeleteEmployee(int id, string status)
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
                    SendBackEmail(employee, status);
                    return Ok("Data deleted");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region HttpDeleteEmployee Helper
        private void SendBackEmail(Employee employee, string status)
        {
            var emailAddress = new EmailAddress(employee.Name, employee.Email);
            var emailMessage = new EmailMessage();

            try
            {
                emailMessage.FromAddresses.Add(new EmailAddress());
                emailMessage.ToAddresses.Add(emailAddress);

                if (status.Equals("yes"))
                {
                    emailMessage.Subject = "Zaakceptowano twoją prośbę o pracę";

                    string pathFile1 = @"Services\EmailService\EmailText\EmailBackYesToEmployee.txt";
                    using var fileStream = new FileStream(pathFile1, FileMode.Open, FileAccess.Read);
                    using var streamReader = new StreamReader(fileStream, Encoding.UTF8);

                    emailMessage.Content = streamReader.ReadToEnd();
                }
                else if (status.Equals("no"))
                {
                    emailMessage.Subject = "Odrzucono twoją prośbę o pracę";

                    string pathFile2 = @"Services\EmailService\EmailText\EmailBackNoToEmployee.txt";
                    using var fileStream = new FileStream(pathFile2, FileMode.Open, FileAccess.Read);
                    using var streamReader = new StreamReader(fileStream, Encoding.UTF8);

                    emailMessage.Content = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                ExceptionAlert(e.Message);
            }

            _emailProvider.Send(emailMessage);
        }
        #endregion

        // GET: api/Employees/error
        [HttpGet("error")]
        public IActionResult ExceptionAlert(string error)
        {
            return BadRequest(error);
        }
    }
}
