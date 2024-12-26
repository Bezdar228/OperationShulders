using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationsController : Controller
    {
        private readonly AppDbContext _context;

        public SpecializationsController(AppDbContext context)
        {
            _context = context;
        }

        // Получить все записи
        [HttpGet]
        public async Task<IActionResult> GetAllSpec()
        {
            var specialization = await _context.Specializations.ToListAsync();
            return Ok(specialization);
        }
    }
}
   
