using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class PermissaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissaoController> _logger;

        public PermissaoController(ApplicationDbContext context, ILogger<PermissaoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Permissao
        public async Task<IActionResult> Index()
        {
            var permissoes = await _context.Permissoes
                .Include(p => p.Categoria)
                .Where(p => p.Ativa)
                .OrderBy(p => p.Categoria.Ordem)
                .ThenBy(p => p.Ordem)
                .ToListAsync();

            return View(permissoes);
        }

        // GET: Permissao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissao = await _context.Permissoes
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (permissao == null)
            {
                return NotFound();
            }

            return View(permissao);
        }

        // GET: Permissao/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categorias = await _context.Categorias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Ordem)
                .ToListAsync();
            return View();
        }

        // POST: Permissao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Permissao permissao)
        {
            if (ModelState.IsValid)
            {
                permissao.DataCriacao = DateTime.Now;
                _context.Add(permissao);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Nova permissão criada: {Nome}", permissao.Nome);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = await _context.Categorias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Ordem)
                .ToListAsync();
            return View(permissao);
        }

        // GET: Permissao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissao = await _context.Permissoes.FindAsync(id);
            if (permissao == null)
            {
                return NotFound();
            }

            ViewBag.Categorias = await _context.Categorias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Ordem)
                .ToListAsync();
            return View(permissao);
        }

        // POST: Permissao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Permissao permissao)
        {
            if (id != permissao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    permissao.DataAtualizacao = DateTime.Now;
                    _context.Update(permissao);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Permissão atualizada: {Nome}", permissao.Nome);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissaoExists(permissao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = await _context.Categorias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Ordem)
                .ToListAsync();
            return View(permissao);
        }

        // GET: Permissao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissao = await _context.Permissoes
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (permissao == null)
            {
                return NotFound();
            }

            return View(permissao);
        }

        // POST: Permissao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permissao = await _context.Permissoes.FindAsync(id);
            if (permissao != null)
            {
                permissao.Ativa = false;
                permissao.DataAtualizacao = DateTime.Now;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Permissão desativada: {Nome}", permissao.Nome);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PermissaoExists(int id)
        {
            return _context.Permissoes.Any(e => e.Id == id);
        }
    }
}
