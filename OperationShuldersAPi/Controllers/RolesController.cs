using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationShuldersAPi.Models;

namespace OperationShuldersAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound("Роль не найдена.");
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoleById), new { id = role.RoleId }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] Role updatedRole)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound("Роль не найдена.");

            role.RoleName = updatedRole.RoleName;

            await _context.SaveChangesAsync();
            return Ok(role);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound("Роль не найдена.");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Роль удалена." });
        }
    }
}
