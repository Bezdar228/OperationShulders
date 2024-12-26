using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        // Получить все записи
        [HttpGet]
        public async Task<IActionResult> GetAllSpec()
        {
            var role = await _context.Roles.ToListAsync();
            return Ok(role);
        }
    }
}
