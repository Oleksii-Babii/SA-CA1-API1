using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BonsaiAPI.Data;
using BonsaiAPI.Models;

namespace BonsaiAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TreesController : ControllerBase
    {
        private readonly BonsaiContext _context;

        public TreesController(BonsaiContext context)
        {
            _context = context;
        }

        // GET: api/trees
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Tree>>> GetAllTrees()
        {
            return await _context.Trees
                .Include(t => t.Species)
                .OrderBy(t => t.Nickname)
                .ToListAsync();
        }

        // GET: api/trees/1
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tree>> GetTree([FromRoute] int id)
        {
            var tree = await _context.Trees.Include(t => t.Species).FirstOrDefaultAsync(t => t.Id == id);
            if (tree == null)
            {
                return NotFound();
            }
            return Ok(tree);
        }

        // POST: api/trees
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Tree>> PostTree([FromBody] Tree tree)
        {
            _context.Trees.Add(tree);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTree), new { id = tree.Id }, tree);
        }
    }
}