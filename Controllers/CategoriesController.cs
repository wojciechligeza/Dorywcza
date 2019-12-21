using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Dorywcza.Data;
using Microsoft.AspNetCore.Cors;

namespace Dorywcza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var category = _context.Categories.ToList();
                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Categories/1
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(a => a.CategoryId == id);

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
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(a => a.CategoryId == id);
        }
    }
}
