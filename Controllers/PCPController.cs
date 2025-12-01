using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class PCPController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PCPController> _logger;

        public PCPController(ApplicationDbContext context, ILogger<PCPController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PCP
        public async Task<IActionResult> Index()
        {
            var ordensProducao = await _context.OrdensProducao
                .Include(o => o.Produto)
                .Include(o => o.CriadoPor)
                .OrderByDescending(o => o.DataCriacao)
                .ToListAsync();
                
            return View(ordensProducao);
        }

        // GET: PCP/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var hoje = DateTime.Today;
            var totalOrdens = await _context.OrdensProducao.CountAsync();
            var ordensEmAndamento = await _context.OrdensProducao.CountAsync(o => o.Status == "EmAndamento");
            var ordensPlanejadas = await _context.OrdensProducao.CountAsync(o => o.Status == "Planejada");
            var ordensConcluidas = await _context.OrdensProducao.CountAsync(o => o.Status == "Concluída");

            ViewBag.TotalOrdens = totalOrdens;
            ViewBag.OrdensEmAndamento = ordensEmAndamento;
            ViewBag.OrdensPlanejadas = ordensPlanejadas;
            ViewBag.OrdensConcluidas = ordensConcluidas;

            var ordensRecentes = await _context.OrdensProducao
                .Include(o => o.Produto)
                .OrderByDescending(o => o.DataCriacao)
                .Take(10)
                .ToListAsync();

            return View(ordensRecentes);
        }

        // GET: PCP/OrdemProducao
        public async Task<IActionResult> OrdemProducao()
        {
            var ordensProducao = await _context.OrdensProducao
                .Include(o => o.Produto)
                .Include(o => o.CriadoPor)
                .Include(o => o.RecursosAlocados)
                    .ThenInclude(r => r.Recurso)
                .OrderByDescending(o => o.Prioridade)
                .ThenByDescending(o => o.DataCriacao)
                .ToListAsync();

            return View(ordensProducao);
        }

        // GET: PCP/CreateOrdem
        public async Task<IActionResult> CreateOrdem()
        {
            ViewBag.Produtos = new SelectList(await _context.Produtos.Where(p => p.Ativo).ToListAsync(), "Id", "Nome");
            ViewBag.Status = new SelectList(new[] { "Planejada", "EmAndamento", "Pausada", "Concluída", "Cancelada" });
            return View();
        }

        // POST: PCP/CreateOrdem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrdem(OrdemProducao ordemProducao)
        {
            if (ModelState.IsValid)
            {
                ordemProducao.DataCriacao = DateTime.Now;
                ordemProducao.CriadoPorId = 1; // TODO: Pegar do usuário logado
                
                _context.Add(ordemProducao);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Ordem de produção criada com sucesso!";
                return RedirectToAction(nameof(OrdemProducao));
            }

            ViewBag.Produtos = new SelectList(await _context.Produtos.Where(p => p.Ativo).ToListAsync(), "Id", "Nome");
            ViewBag.Status = new SelectList(new[] { "Planejada", "EmAndamento", "Pausada", "Concluída", "Cancelada" });
            return View(ordemProducao);
        }

        // GET: PCP/EditOrdem/5
        public async Task<IActionResult> EditOrdem(int? id)
        {
            if (id == null) return NotFound();

            var ordemProducao = await _context.OrdensProducao.FindAsync(id);
            if (ordemProducao == null) return NotFound();

            ViewBag.Produtos = new SelectList(await _context.Produtos.Where(p => p.Ativo).ToListAsync(), "Id", "Nome", ordemProducao.ProdutoId);
            ViewBag.Status = new SelectList(new[] { "Planejada", "EmAndamento", "Pausada", "Concluída", "Cancelada" }, ordemProducao.Status);
            return View(ordemProducao);
        }

        // POST: PCP/EditOrdem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrdem(int id, OrdemProducao ordemProducao)
        {
            if (id != ordemProducao.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordemProducao);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Ordem de produção atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdemProducaoExists(ordemProducao.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(OrdemProducao));
            }

            ViewBag.Produtos = new SelectList(await _context.Produtos.Where(p => p.Ativo).ToListAsync(), "Id", "Nome", ordemProducao.ProdutoId);
            ViewBag.Status = new SelectList(new[] { "Planejada", "EmAndamento", "Pausada", "Concluída", "Cancelada" }, ordemProducao.Status);
            return View(ordemProducao);
        }

        // GET: PCP/Recursos
        public async Task<IActionResult> Recursos()
        {
            var recursos = await _context.Recursos
                .OrderBy(r => r.Tipo)
                .ThenBy(r => r.Nome)
                .ToListAsync();
            return View(recursos);
        }

        // GET: PCP/CreateRecurso
        public IActionResult CreateRecurso()
        {
            ViewBag.Tipos = new SelectList(new[] { "Maquina", "Ferramenta", "MaoDeObra", "Equipamento" });
            return View();
        }

        // POST: PCP/CreateRecurso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecurso(Recurso recurso)
        {
            if (ModelState.IsValid)
            {
                recurso.DataCadastro = DateTime.Now;
                _context.Add(recurso);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Recurso criado com sucesso!";
                return RedirectToAction(nameof(Recursos));
            }

            ViewBag.Tipos = new SelectList(new[] { "Maquina", "Ferramenta", "MaoDeObra", "Equipamento" });
            return View(recurso);
        }

        // GET: PCP/Apontamento
        public async Task<IActionResult> Apontamento()
        {
            var ordensEmAndamento = await _context.OrdensProducao
                .Include(o => o.Produto)
                .Where(o => o.Status == "EmAndamento" || o.Status == "Planejada")
                .OrderBy(o => o.Prioridade)
                .ToListAsync();

            return View(ordensEmAndamento);
        }

        // GET: PCP/CreateApontamento/5
        public async Task<IActionResult> CreateApontamento(int? id)
        {
            if (id == null) return NotFound();

            var ordemProducao = await _context.OrdensProducao
                .Include(o => o.Produto)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordemProducao == null) return NotFound();

            var apontamento = new ApontamentoProducao
            {
                OrdemProducaoId = ordemProducao.Id,
                DataHoraApontamento = DateTime.Now
            };

            ViewBag.OrdemProducao = ordemProducao;
            return View(apontamento);
        }

        // POST: PCP/CreateApontamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApontamento(ApontamentoProducao apontamento)
        {
            if (ModelState.IsValid)
            {
                apontamento.OperadorId = 1; // TODO: Pegar do usuário logado
                apontamento.DataHoraApontamento = DateTime.Now;
                
                _context.Add(apontamento);

                // Atualizar quantidade produzida na ordem
                var ordem = await _context.OrdensProducao.FindAsync(apontamento.OrdemProducaoId);
                if (ordem != null)
                {
                    ordem.QuantidadeProduzida += apontamento.QuantidadeProduzida;
                    
                    // Se atingiu a quantidade, marcar como concluída
                    if (ordem.QuantidadeProduzida >= ordem.Quantidade)
                    {
                        ordem.Status = "Concluída";
                        ordem.DataFimReal = DateTime.Now;
                    }
                    else if (ordem.Status == "Planejada")
                    {
                        ordem.Status = "EmAndamento";
                        ordem.DataInicioReal = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Apontamento registrado com sucesso!";
                return RedirectToAction(nameof(Apontamento));
            }

            var ordemProducao = await _context.OrdensProducao
                .Include(o => o.Produto)
                .FirstOrDefaultAsync(o => o.Id == apontamento.OrdemProducaoId);
            
            ViewBag.OrdemProducao = ordemProducao;
            return View(apontamento);
        }

        private bool OrdemProducaoExists(int id)
        {
            return _context.OrdensProducao.Any(e => e.Id == id);
        }
    }
}
