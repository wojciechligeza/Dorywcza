using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class JobOffersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobOffersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JobOffers
        [HttpGet]
        public async Task<IActionResult> GetJobOffers()
        {
            try
            {
                var jobOffers = await _context.JobOffers
                    .Include(a => a.JobOfferEmployees)
                    .ThenInclude(b => b.Employee).ToListAsync();

                return Ok(jobOffers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/JobOffers/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobOffer(int id)
        {
            try
            {
                var jobOffer = await _context.JobOffers
                    .Include(a => a.JobOfferEmployees)
                    .ThenInclude(b => b.Employee).FirstOrDefaultAsync(a => a.JobOfferId == id);

                if (jobOffer == null)
                {
                    return NotFound();
                }
                return Ok(jobOffer);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/JobOffers/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobOffer(int id, [FromBody]JobOffer jobOffer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != jobOffer.JobOfferId) return BadRequest();

            try
            {
                if ((_context.JobOffers.FirstOrDefault(a => a.JobOfferId == id)) == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Entry(jobOffer).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok("Data updated");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/JobOffers
        [HttpPost]
        public async Task<IActionResult> PostJobOffer([FromBody]JobOffer jobOffer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _context.JobOffers.Add(jobOffer);
                await _context.SaveChangesAsync();
                return Ok("Data created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/JobOffers/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobOffer(int id)
        {
            try
            {
                var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(a => a.JobOfferId == id);
                if (jobOffer == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.JobOffers.Remove(jobOffer);
                    await _context.SaveChangesAsync();
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
