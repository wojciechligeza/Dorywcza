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
    public class EmployersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployersController(ApplicationDbContext context)
        {
            _context = context;
        }

         // GET: api/Employers
        [HttpGet]
        public IActionResult GetEmployers()
        {
            try
            {
                var employers = _context.Employers.ToList();
                return Ok(employers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Employers/1
        [HttpGet("{id}")]
        public IActionResult GetEmployer(int id)
        {
            try
            {
                var employer = _context.Employers.FirstOrDefault(a => a.EmployerId == id);

                if (employer == null)
                {
                    return NotFound();
                }
                return Ok(employer);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Employers/1
        [HttpPut("{id}")]
        public IActionResult PutEmployer(int id, [FromBody]Employer employer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != employer.EmployerId) return BadRequest();

            try
            {
                if ((_context.Employers.FirstOrDefault(a => a.EmployerId == id)) == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Entry(employer).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok("Data updated");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Employers
        [HttpPost]
        public IActionResult PostEmployer([FromBody]Employer employer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _context.Employers.Add(employer);
                _context.SaveChanges();
                return Ok("Data created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Employers/1
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployer(int id)
        {
            try
            {
                var employer = _context.Employers.FirstOrDefault(a => a.EmployerId == id);
                if (employer == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Employers.Remove(employer);
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
