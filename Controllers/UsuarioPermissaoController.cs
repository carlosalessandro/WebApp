using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class UsuarioPermissaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsuarioPermissaoController> _logger;

        public UsuarioPermissaoController(ApplicationDbContext context, ILogger<UsuarioPermissaoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: UsuarioPermissao
        public async Task<IActionResult> Index()
        {
            var usuarioPermissoes = await _context.UsuarioPermissoes
                .Include(up => up.Usuario)
                .Include(up => up.Permissao)
                .ThenInclude(p => p.Categoria)
                .OrderBy(up => up.Usuario.Name)
                .ThenBy(up => up.Permissao.Categoria.Ordem)
                .ThenBy(up => up.Permissao.Ordem)
                .ToListAsync();

            return View(usuarioPermissoes);
        }

        // GET: UsuarioPermissao/Manage/5
        public async Task<IActionResult> Manage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Buscar todas as permissões agrupadas por categoria
            var permissoesPorCategoria = await _context.Permissoes
                .Include(p => p.Categoria)
                .Where(p => p.Ativa)
                .OrderBy(p => p.Categoria.Ordem)
                .ThenBy(p => p.Ordem)
                .GroupBy(p => p.Categoria)
                .ToListAsync();

            // Buscar permissões já concedidas ao usuário
            var permissoesConcedidas = await _context.UsuarioPermissoes
                .Where(up => up.UsuarioId == id && up.Concedida)
                .Select(up => up.PermissaoId)
                .ToListAsync();

            ViewBag.Usuario = usuario;
            ViewBag.PermissoesPorCategoria = permissoesPorCategoria;
            ViewBag.PermissoesConcedidas = permissoesConcedidas;

            return View();
        }

        // POST: UsuarioPermissao/Manage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(int id, List<int> permissaoIds)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            try
            {
                // Remover todas as permissões atuais do usuário
                var permissoesAtuais = await _context.UsuarioPermissoes
                    .Where(up => up.UsuarioId == id)
                    .ToListAsync();

                _context.UsuarioPermissoes.RemoveRange(permissoesAtuais);

                // Adicionar as novas permissões
                if (permissaoIds != null && permissaoIds.Any())
                {
                    var novasPermissoes = permissaoIds.Select(permissaoId => new UsuarioPermissao
                    {
                        UsuarioId = id,
                        PermissaoId = permissaoId,
                        Concedida = true,
                        DataConcessao = DateTime.Now
                    }).ToList();

                    _context.UsuarioPermissoes.AddRange(novasPermissoes);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Permissões atualizadas para o usuário: {Nome}", usuario.Name);
                TempData["SuccessMessage"] = "Permissões atualizadas com sucesso!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar permissões do usuário: {Nome}", usuario.Name);
                TempData["ErrorMessage"] = "Erro ao atualizar permissões. Tente novamente.";
            }

            return RedirectToAction(nameof(Manage), new { id });
        }

        // GET: UsuarioPermissao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioPermissao = await _context.UsuarioPermissoes
                .Include(up => up.Usuario)
                .Include(up => up.Permissao)
                .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuarioPermissao == null)
            {
                return NotFound();
            }

            return View(usuarioPermissao);
        }

        // GET: UsuarioPermissao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioPermissao = await _context.UsuarioPermissoes
                .Include(up => up.Usuario)
                .Include(up => up.Permissao)
                .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuarioPermissao == null)
            {
                return NotFound();
            }

            return View(usuarioPermissao);
        }

        // POST: UsuarioPermissao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioPermissao = await _context.UsuarioPermissoes.FindAsync(id);
            if (usuarioPermissao != null)
            {
                _context.UsuarioPermissoes.Remove(usuarioPermissao);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Permissão removida do usuário: {UsuarioId}", usuarioPermissao.UsuarioId);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
