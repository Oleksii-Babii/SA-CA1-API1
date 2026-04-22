using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BonsaiAPI.Data;
using BonsaiAPI.Models;

namespace BonsaiAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly BonsaiContext _context;

        public SpeciesController(BonsaiContext context)
        {
            _context = context;
        }

        // GET: api/species
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Species>>> GetAllSpecies()
        {
            return await _context.Species.OrderBy(s => s.Name).ToListAsync();
        }
    }
}