using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ThemeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThemeConfig>>> GetThemes()
        {
            return await _context.ThemeConfigs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ThemeConfig>> GetTheme(int id)
        {
            var theme = await _context.ThemeConfigs.FindAsync(id);

            if (theme == null)
            {
                return NotFound();
            }

            return theme;
        }

        [HttpGet("active")]
        public async Task<ActionResult<ThemeConfig>> GetActiveTheme()
        {
            var theme = await _context.ThemeConfigs
                .FirstOrDefaultAsync(t => t.IsActive);

            if (theme == null)
            {
                return NotFound(new { message = "Nenhum tema ativo encontrado" });
            }

            return theme;
        }

        [HttpPost]
        public async Task<ActionResult<ThemeConfig>> CreateTheme(ThemeConfig theme)
        {
            theme.CreatedAt = DateTime.UtcNow;
            _context.ThemeConfigs.Add(theme);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTheme), new { id = theme.Id }, theme);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheme(int id, ThemeConfig theme)
        {
            if (id != theme.Id)
            {
                return BadRequest();
            }

            theme.UpdatedAt = DateTime.UtcNow;
            _context.Entry(theme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThemeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateTheme(int id)
        {
            var theme = await _context.ThemeConfigs.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            // Desativar todos os temas
            var activeThemes = await _context.ThemeConfigs
                .Where(t => t.IsActive)
                .ToListAsync();

            foreach (var t in activeThemes)
            {
                t.IsActive = false;
            }

            // Ativar o tema selecionado
            theme.IsActive = true;
            theme.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(theme);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheme(int id)
        {
            var theme = await _context.ThemeConfigs.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            if (theme.IsActive)
            {
                return BadRequest(new { message = "Não é possível excluir o tema ativo" });
            }

            _context.ThemeConfigs.Remove(theme);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThemeExists(int id)
        {
            return _context.ThemeConfigs.Any(e => e.Id == id);
        }
    }
}
