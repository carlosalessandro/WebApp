using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Models.ERP;
using System.Text.Json;

namespace WebApp.Controllers
{
    // [Authorize] // Comentado temporariamente para testes
    public class ERPController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ERPController> _logger;

        public ERPController(ApplicationDbContext context, ILogger<ERPController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Dashboard ERP
        public async Task<IActionResult> Index()
        {
            try
            {
                var hoje = DateTime.Today;
                var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
                var fimMes = inicioMes.AddMonths(1).AddDays(-1);

                // Estatísticas Financeiras
                var totalContasPagar = await _context.ContasPagar
                    .Where(c => c.Status == StatusConta.Aberta)
                    .SumAsync(c => c.ValorSaldo);

                var totalContasReceber = await _context.ContasReceber
                    .Where(c => c.Status == StatusConta.Aberta)
                    .SumAsync(c => c.ValorSaldo);

                var fluxoCaixaMes = await _context.MovimentacoesFinanceiras
                    .Where(m => m.DataMovimentacao >= inicioMes && m.DataMovimentacao <= fimMes)
                    .SumAsync(m => m.Tipo == TipoMovimentacao.Entrada ? m.Valor : -m.Valor);

                // Estatísticas de Produção
                var opsEmAndamento = await _context.OrdensProducaoCompletas
                    .CountAsync(o => o.Status == StatusOP.EmProducao);

                var opsAtrasadas = await _context.OrdensProducaoCompletas
                    .CountAsync(o => o.Atrasada);

                var produtividadeMedia = await _context.ApontamentosProducaoCompletos
                    .Where(a => a.Status == StatusApontamento.Concluido)
                    .AverageAsync(a => a.ProdutividadeHoraria);

                // Estatísticas de Recursos
                var recursosDisponiveis = await _context.RecursosProducao
                    .CountAsync(r => r.Status == StatusRecurso.Disponivel);

                var recursosManutencao = await _context.RecursosProducao
                    .CountAsync(r => r.Status == StatusRecurso.Manutencao);

                // Qualidade
                var taxaAprovacaoQualidade = await _context.InspecoesQualidade
                    .Where(i => i.DataInspecao >= inicioMes)
                    .AverageAsync(i => i.TaxaAprovacao);

                ViewBag.TotalContasPagar = totalContasPagar;
                ViewBag.TotalContasReceber = totalContasReceber;
                ViewBag.FluxoCaixaMes = fluxoCaixaMes;
                ViewBag.OPsEmAndamento = opsEmAndamento;
                ViewBag.OPsAtrasadas = opsAtrasadas;
                ViewBag.ProdutividadeMedia = produtividadeMedia;
                ViewBag.RecursosDisponiveis = recursosDisponiveis;
                ViewBag.RecursosManutencao = recursosManutencao;
                ViewBag.TaxaAprovacaoQualidade = taxaAprovacaoQualidade;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar dashboard ERP");
                return View("Error");
            }
        }

        // Plano de Contas
        public async Task<IActionResult> PlanoContas()
        {
            var planoContas = await _context.PlanoContas
                .Include(pc => pc.Pai)
                .OrderBy(pc => pc.CodigoCompleto)
                .ToListAsync();

            return View(planoContas);
        }

        public IActionResult ContaCreate()
        {
            ViewBag.ContasPai = _context.PlanoContas.Where(pc => !pc.Analitica).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContaCreate(PlanoContas conta)
        {
            if (ModelState.IsValid)
            {
                // Calcular nível e código completo
                if (conta.PaiId.HasValue)
                {
                    var pai = await _context.PlanoContas.FindAsync(conta.PaiId.Value);
                    if (pai != null)
                        conta.Nivel = pai.Nivel + 1;
                    else
                        conta.Nivel = 1;
                }
                else
                {
                    conta.Nivel = 1;
                }

                conta.DataCriacao = DateTime.Now;
                _context.PlanoContas.Add(conta);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Conta {conta.Id} criada com sucesso");
                return RedirectToAction(nameof(PlanoContas));
            }

            ViewBag.ContasPai = _context.PlanoContas.Where(pc => !pc.Analitica).ToList();
            return View(conta);
        }

        // Lançamentos Contábeis
        public async Task<IActionResult> LancamentosContabeis()
        {
            var lancamentos = await _context.LancamentosContabeis
                .Include(l => l.ContaDebito)
                .Include(l => l.ContaCredito)
                .Include(l => l.CentroCusto)
                .Include(l => l.Projeto)
                .Include(l => l.CriadoPor)
                .OrderByDescending(l => l.DataLancamento)
                .ToListAsync();

            return View(lancamentos);
        }

        public IActionResult LancamentoCreate()
        {
            ViewBag.Contas = _context.PlanoContas.Where(pc => pc.Ativa).OrderBy(pc => pc.CodigoCompleto).ToList();
            ViewBag.CentrosCusto = _context.CentrosCusto.Where(cc => cc.Ativo).ToList();
            ViewBag.Projetos = _context.Projetos.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LancamentoCreate(LancamentoContabil lancamento)
        {
            if (ModelState.IsValid)
            {
                lancamento.DataCriacao = DateTime.Now;
                lancamento.NumeroDocumento = GerarNumeroDocumento();
                _context.LancamentosContabeis.Add(lancamento);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Lançamento {lancamento.Id} criado com sucesso");
                return RedirectToAction(nameof(LancamentosContabeis));
            }

            ViewBag.Contas = _context.PlanoContas.Where(pc => pc.Ativa).OrderBy(pc => pc.CodigoCompleto).ToList();
            ViewBag.CentrosCusto = _context.CentrosCusto.Where(cc => cc.Ativo).ToList();
            ViewBag.Projetos = _context.Projetos.ToList();
            return View(lancamento);
        }

        // Produção
        public async Task<IActionResult> OrdensProducao()
        {
            var ordens = await _context.OrdensProducaoCompletas
                .Include(op => op.Produto)
                .Include(op => op.Cliente)
                .Include(op => op.GerenteProducao)
                .OrderByDescending(op => op.DataEmissao)
                .ToListAsync();

            return View(ordens);
        }

        public async Task<IActionResult> OrdemProducaoDetails(int? id)
        {
            if (id == null) return NotFound();

            var ordem = await _context.OrdensProducaoCompletas
                .Include(op => op.Produto)
                .Include(op => op.Cliente)
                .Include(op => op.GerenteProducao)
                .Include(op => op.Apontamentos.OrderByDescending(a => a.DataHoraInicio))
                .ThenInclude(a => a.Recurso)
                .Include(op => op.Apontamentos)
                .ThenInclude(a => a.Operador)
                .Include(op => op.RecursosAlocados)
                .ThenInclude(ra => ra.Recurso)
                .Include(op => op.Inspecoes.OrderByDescending(i => i.DataInspecao))
                .ThenInclude(i => i.Inspetor)
                .FirstOrDefaultAsync(op => op.Id == id);

            if (ordem == null) return NotFound();

            return View(ordem);
        }

        public IActionResult OrdemProducaoCreate()
        {
            ViewBag.Produtos = _context.Produtos.Where(p => p.Ativo).ToList();
            ViewBag.Clientes = _context.Clientes.Where(c => c.Ativo).ToList();
            ViewBag.Usuarios = _context.Users.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrdemProducaoCreate(OrdemProducaoCompleta ordem)
        {
            if (ModelState.IsValid)
            {
                ordem.NumeroOP = GerarNumeroOP();
                ordem.DataEmissao = DateTime.Now;
                ordem.Status = StatusOP.Planejada;
                _context.OrdensProducaoCompletas.Add(ordem);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"OP {ordem.NumeroOP} criada com sucesso");
                return RedirectToAction(nameof(OrdensProducao));
            }

            ViewBag.Produtos = _context.Produtos.Where(p => p.Ativo).ToList();
            ViewBag.Clientes = _context.Clientes.Where(c => c.Ativo).ToList();
            ViewBag.Usuarios = _context.Users.ToList();
            return View(ordem);
        }

        // Recursos de Produção
        public async Task<IActionResult> RecursosProducao()
        {
            var recursos = await _context.RecursosProducao
                .Include(r => r.Alocacoes)
                .ThenInclude(a => a.OrdemProducao)
                .OrderBy(r => r.Nome)
                .ToListAsync();

            return View(recursos);
        }

        public IActionResult RecursoCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecursoCreate(RecursoProducao recurso)
        {
            if (ModelState.IsValid)
            {
                recurso.DataCriacao = DateTime.Now;
                recurso.Status = StatusRecurso.Disponivel;
                _context.RecursosProducao.Add(recurso);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Recurso {recurso.Id} criado com sucesso");
                return RedirectToAction(nameof(RecursosProducao));
            }

            return View(recurso);
        }

        // Qualidade
        public async Task<IActionResult> InspecoesQualidade()
        {
            var inspecoes = await _context.InspecoesQualidade
                .Include(i => i.OrdemProducao)
                .ThenInclude(op => op.Produto)
                .Include(i => i.Inspetor)
                .OrderByDescending(i => i.DataInspecao)
                .ToListAsync();

            return View(inspecoes);
        }

        // API para gráficos
        [HttpGet]
        public async Task<IActionResult> GetFluxoCaixaUltimos30Dias()
        {
            var dataInicio = DateTime.Today.AddDays(-30);
            var dados = await _context.MovimentacoesFinanceiras
                .Where(m => m.DataMovimentacao >= dataInicio)
                .GroupBy(m => new { Ano = m.DataMovimentacao.Year, Mes = m.DataMovimentacao.Month, Dia = m.DataMovimentacao.Day })
                .OrderBy(g => g.Key.Ano).ThenBy(g => g.Key.Mes).ThenBy(g => g.Key.Dia)
                .Select(g => new 
                { 
                    Data = $"{g.Key.Dia}/{g.Key.Mes}", 
                    Entradas = g.Where(m => m.Tipo == TipoMovimentacao.Entrada).Sum(m => m.Valor),
                    Saidas = g.Where(m => m.Tipo == TipoMovimentacao.Saida).Sum(m => m.Valor)
                })
                .ToListAsync();

            return Json(dados);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducaoPorStatus()
        {
            var dados = await _context.OrdensProducaoCompletas
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key.ToString(), Quantidade = g.Count() })
                .ToListAsync();

            return Json(dados);
        }

        [HttpGet]
        public async Task<IActionResult> GetOcupacaoRecursos()
        {
            var dados = await _context.RecursosProducao
                .GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key.ToString(), Quantidade = g.Count() })
                .ToListAsync();

            return Json(dados);
        }

        // Métodos auxiliares
        private string GerarNumeroDocumento()
        {
            var data = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random().Next(1000, 9999);
            return $"DOC{data}{random}";
        }

        private string GerarNumeroOP()
        {
            var data = DateTime.Now.ToString("yyyyMM");
            var sequencia = (_context.OrdensProducaoCompletas.CountAsync().Result + 1).ToString("0000");
            return $"OP{data}{sequencia}";
        }
    }
}
