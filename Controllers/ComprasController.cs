using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComprasController> _logger;

        public ComprasController(ApplicationDbContext context, ILogger<ComprasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            try
            {
                var pedidos = await _context.PedidosCompra
                    .Include(p => p.Fornecedor)
                    .Include(p => p.Itens)!
                        .ThenInclude(i => i.Produto)
                    .OrderByDescending(p => p.DataPedido)
                    .ToListAsync();

                return View(pedidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar lista de pedidos de compra");
                TempData["ErrorMessage"] = "Ocorreu um erro ao carregar os pedidos de compra. Tente novamente.";
                return View(new List<PedidoCompra>());
            }
        }
    }
}
