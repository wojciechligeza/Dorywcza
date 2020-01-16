using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dorywcza.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace Dorywcza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("GetOnly")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Include(a => a.JobOffers).ToListAsync();

                return Ok(categories);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Categories/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(a => a.JobOffers).FirstOrDefaultAsync(a => a.CategoryId == id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
