using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Users
                .Include(u => u.UsuarioPermissoes)
                .ThenInclude(up => up.Permissao)
                .ThenInclude(p => p.Categoria)
                .OrderBy(u => u.Name)
                .ToListAsync();

            return View(usuarios);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.UsuarioPermissoes)
                .ThenInclude(up => up.Permissao)
                .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Permissions/5
        public async Task<IActionResult> Permissions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Buscar permissões do usuário agrupadas por categoria
            var permissoesPorCategoria = await _context.UsuarioPermissoes
                .Include(up => up.Permissao)
                .ThenInclude(p => p.Categoria)
                .Where(up => up.UsuarioId == id && up.Concedida)
                .OrderBy(up => up.Permissao.Categoria.Ordem)
                .ThenBy(up => up.Permissao.Ordem)
                .GroupBy(up => up.Permissao.Categoria)
                .ToListAsync();

            ViewBag.Usuario = user;
            ViewBag.PermissoesPorCategoria = permissoesPorCategoria;

            return View();
        }

        // GET: User/ManagePermissions/5
        public async Task<IActionResult> ManagePermissions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
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

            ViewBag.Usuario = user;
            ViewBag.PermissoesPorCategoria = permissoesPorCategoria;
            ViewBag.PermissoesConcedidas = permissoesConcedidas;

            return View();
        }

        // POST: User/ManagePermissions/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermissions(int id, List<int> permissaoIds)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
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
                _logger.LogInformation("Permissões atualizadas para o usuário: {Nome}", user.Name);
                TempData["SuccessMessage"] = "Permissões atualizadas com sucesso!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar permissões do usuário: {Nome}", user.Name);
                TempData["ErrorMessage"] = "Erro ao atualizar permissões. Tente novamente.";
            }

            return RedirectToAction(nameof(ManagePermissions), new { id });
        }
    }
}
