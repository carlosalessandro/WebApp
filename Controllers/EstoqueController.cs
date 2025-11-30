using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EstoqueController> _logger;

        public EstoqueController(ApplicationDbContext context, ILogger<EstoqueController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Dashboard de Estoque
        public async Task<IActionResult> Index()
        {
            // Carrega em memória para evitar limitações do SQLite com Sum em decimal
            var estoques = await _context.Estoques.ToListAsync();

            var dashboard = new
            {
                TotalProdutos = estoques.Count,
                ProdutosEstoqueBaixo = estoques.Count(e => e.QuantidadeAtual <= e.QuantidadeMinima),
                ProdutosSemEstoque = estoques.Count(e => e.QuantidadeAtual <= 0),
                ValorTotalEstoque = estoques.Sum(e => e.QuantidadeAtual * e.CustoMedio)
            };

            var produtosEstoqueBaixo = (await _context.Estoques
                .Include(e => e.Produto)
                .Where(e => e.QuantidadeAtual <= e.QuantidadeMinima)
                .ToListAsync())
                .OrderBy(e => e.QuantidadeAtual)
                .Take(10)
                .ToList();

            ViewBag.Dashboard = dashboard;
            ViewBag.ProdutosEstoqueBaixo = produtosEstoqueBaixo;

            return View();
        }

        // Consulta de Estoque
        public async Task<IActionResult> Consulta(string? filtro)
        {
            var query = _context.Estoques
                .Include(e => e.Produto)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(e => (e.Produto != null && e.Produto.Nome.Contains(filtro)) || 
                                        (e.Produto != null && e.Produto.Codigo.Contains(filtro)));
            }

            var estoques = (await query.ToListAsync())
                .OrderBy(e => e.Produto?.Nome ?? "")
                .ToList();

            ViewBag.Filtro = filtro;
            return View(estoques);
        }

        // Movimentações de Estoque
        public async Task<IActionResult> Movimentacoes(int? produtoId, DateTime? dataInicio, DateTime? dataFim)
        {
            var query = _context.MovimentacoesEstoque
                .Include(m => m.Estoque)
                .ThenInclude(e => e!.Produto)
                .Include(m => m.Usuario)
                .AsQueryable();

            if (produtoId.HasValue)
            {
                query = query.Where(m => m.Estoque!.ProdutoId == produtoId.Value);
            }

            if (dataInicio.HasValue)
            {
                query = query.Where(m => m.DataMovimentacao >= dataInicio.Value);
            }

            if (dataFim.HasValue)
            {
                query = query.Where(m => m.DataMovimentacao <= dataFim.Value);
            }

            var movimentacoes = await query
                .OrderByDescending(m => m.DataMovimentacao)
                .Take(100)
                .ToListAsync();

            ViewBag.Produtos = (await _context.Produtos
                .Select(p => new { p.Id, p.Nome })
                .ToListAsync())
                .OrderBy(p => p.Nome)
                .ToList();

            ViewBag.ProdutoId = produtoId;
            ViewBag.DataInicio = dataInicio?.ToString("yyyy-MM-dd");
            ViewBag.DataFim = dataFim?.ToString("yyyy-MM-dd");

            return View(movimentacoes);
        }

        // Entrada de Estoque
        [HttpGet]
        public async Task<IActionResult> Entrada()
        {
            ViewBag.Produtos = (await _context.Produtos
                .ToListAsync())
                .OrderBy(p => p.Nome)
                .ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Entrada(int produtoId, decimal quantidade, decimal? custoUnitario, string motivo, string? observacoes)
        {
            try
            {
                var estoque = await _context.Estoques
                    .FirstOrDefaultAsync(e => e.ProdutoId == produtoId);

                if (estoque == null)
                {
                    // Criar estoque se não existir
                    estoque = new Estoque
                    {
                        ProdutoId = produtoId,
                        QuantidadeAtual = 0,
                        QuantidadeMinima = 0,
                        QuantidadeMaxima = 1000,
                        CustoMedio = custoUnitario ?? 0,
                        UltimoCusto = custoUnitario ?? 0
                    };
                    _context.Estoques.Add(estoque);
                    await _context.SaveChangesAsync();
                }

                // Atualizar custo médio se informado
                if (custoUnitario.HasValue && custoUnitario.Value > 0)
                {
                    var valorAnterior = estoque.QuantidadeAtual * estoque.CustoMedio;
                    var valorNovo = quantidade * custoUnitario.Value;
                    var quantidadeTotal = estoque.QuantidadeAtual + quantidade;

                    if (quantidadeTotal > 0)
                    {
                        estoque.CustoMedio = (valorAnterior + valorNovo) / quantidadeTotal;
                    }

                    estoque.UltimoCusto = custoUnitario.Value;
                }

                // Atualizar quantidade
                estoque.QuantidadeAtual += quantidade;
                estoque.DataAtualizacao = DateTime.Now;

                // Registrar movimentação
                var movimentacao = new MovimentacaoEstoque
                {
                    EstoqueId = estoque.Id,
                    Tipo = TipoMovimentacaoEstoque.Entrada,
                    Quantidade = quantidade,
                    CustoUnitario = custoUnitario,
                    Motivo = motivo,
                    Observacoes = observacoes,
                    UsuarioId = 1 // TODO: Pegar do usuário logado
                };

                _context.MovimentacoesEstoque.Add(movimentacao);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Entrada de estoque registrada com sucesso!";
                return RedirectToAction(nameof(Consulta));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar entrada de estoque");
                TempData["Error"] = "Erro ao registrar entrada de estoque.";
            }

            ViewBag.Produtos = (await _context.Produtos
                .ToListAsync())
                .OrderBy(p => p.Nome)
                .ToList();

            return View();
        }

        // Saída de Estoque
        [HttpGet]
        public async Task<IActionResult> Saida()
        {
            ViewBag.Produtos = (await _context.Produtos
                .ToListAsync())
                .OrderBy(p => p.Nome)
                .ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Saida(int produtoId, decimal quantidade, string motivo, string? observacoes)
        {
            try
            {
                var estoque = await _context.Estoques
                    .FirstOrDefaultAsync(e => e.ProdutoId == produtoId);

                if (estoque == null)
                {
                    TempData["Error"] = "Produto não encontrado no estoque.";
                    return RedirectToAction(nameof(Saida));
                }

                if (estoque.QuantidadeAtual < quantidade)
                {
                    TempData["Error"] = $"Quantidade insuficiente em estoque. Disponível: {estoque.QuantidadeAtual}";
                    return RedirectToAction(nameof(Saida));
                }

                // Atualizar quantidade
                estoque.QuantidadeAtual -= quantidade;
                estoque.DataAtualizacao = DateTime.Now;

                // Registrar movimentação
                var movimentacao = new MovimentacaoEstoque
                {
                    EstoqueId = estoque.Id,
                    Tipo = TipoMovimentacaoEstoque.Saida,
                    Quantidade = quantidade,
                    CustoUnitario = estoque.CustoMedio,
                    Motivo = motivo,
                    Observacoes = observacoes,
                    UsuarioId = 1 // TODO: Pegar do usuário logado
                };

                _context.MovimentacoesEstoque.Add(movimentacao);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Saída de estoque registrada com sucesso!";
                return RedirectToAction(nameof(Consulta));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar saída de estoque");
                TempData["Error"] = "Erro ao registrar saída de estoque.";
            }

            ViewBag.Produtos = (await _context.Produtos
                .ToListAsync())
                .OrderBy(p => p.Nome)
                .ToList();

            return View();
        }

        // Ajuste de Estoque
        [HttpGet]
        public async Task<IActionResult> Ajuste()
        {
            ViewBag.Produtos = (await _context.Produtos
                .ToListAsync())
                .OrderBy(p => p.Nome)
                .ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ajuste(int produtoId, decimal quantidadeNova, string motivo, string? observacoes)
        {
            try
            {
                var estoque = await _context.Estoques
                    .FirstOrDefaultAsync(e => e.ProdutoId == produtoId);

                if (estoque == null)
                {
                    TempData["Error"] = "Produto não encontrado no estoque.";
                    return RedirectToAction(nameof(Ajuste));
                }

                var quantidadeAnterior = estoque.QuantidadeAtual;
                var diferenca = quantidadeNova - quantidadeAnterior;

                // Atualizar quantidade
                estoque.QuantidadeAtual = quantidadeNova;
                estoque.DataAtualizacao = DateTime.Now;

                // Registrar movimentação
                var movimentacao = new MovimentacaoEstoque
                {
                    EstoqueId = estoque.Id,
                    Tipo = TipoMovimentacaoEstoque.Ajuste,
                    Quantidade = Math.Abs(diferenca),
                    CustoUnitario = estoque.CustoMedio,
                    Motivo = $"{motivo} (Anterior: {quantidadeAnterior}, Nova: {quantidadeNova})",
                    Observacoes = observacoes,
                    UsuarioId = 1 // TODO: Pegar do usuário logado
                };

                _context.MovimentacoesEstoque.Add(movimentacao);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Ajuste de estoque realizado com sucesso!";
                return RedirectToAction(nameof(Consulta));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar ajuste de estoque");
                TempData["Error"] = "Erro ao realizar ajuste de estoque.";
            }

            ViewBag.Produtos = (await _context.Produtos
                .ToListAsync())
                .OrderBy(p => p.Nome)
                .ToList();

            return View();
        }

        // API para obter dados do produto
        [HttpGet]
        public async Task<IActionResult> ObterDadosProduto(int id)
        {
            var estoque = await _context.Estoques
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(e => e.ProdutoId == id);

            if (estoque == null)
            {
                return Json(new { success = false, message = "Produto não encontrado no estoque" });
            }

            return Json(new
            {
                success = true,
                produto = estoque.Produto!.Nome,
                quantidadeAtual = estoque.QuantidadeAtual,
                custoMedio = estoque.CustoMedio,
                ultimoCusto = estoque.UltimoCusto
            });
        }
    }
}
