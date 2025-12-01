using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class FinanceiroController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FinanceiroController> _logger;

        public FinanceiroController(ApplicationDbContext context, ILogger<FinanceiroController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Dashboard Financeiro
        public async Task<IActionResult> Index()
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            // Carrega em memória para evitar problemas de tradução de agregação no SQLite
            var contasPagarMes = await _context.ContasPagar
                .Where(c => c.Status == StatusConta.Aberta &&
                           c.DataVencimento >= inicioMes &&
                           c.DataVencimento <= fimMes)
                .ToListAsync();

            var contasReceberMes = await _context.ContasReceber
                .Where(c => c.Status == StatusConta.Aberta &&
                           c.DataVencimento >= inicioMes &&
                           c.DataVencimento <= fimMes)
                .ToListAsync();

            var contasBancariasAtivas = await _context.ContasBancarias
                .Where(c => c.Ativa)
                .ToListAsync();

            var totalPagarMes = contasPagarMes
                .Sum(c => c.ValorOriginal + c.ValorJuros + c.ValorMulta - c.ValorDesconto);

            var totalReceberMes = contasReceberMes
                .Sum(c => c.ValorOriginal + c.ValorJuros - c.ValorDesconto);

            var saldoContas = contasBancariasAtivas
                .Sum(c => c.SaldoAtual);

            var dashboard = new
            {
                ContasPagarVencidas = await _context.ContasPagar
                    .Where(c => c.Status == StatusConta.Aberta && c.DataVencimento < hoje)
                    .CountAsync(),
                
                ContasReceberVencidas = await _context.ContasReceber
                    .Where(c => c.Status == StatusConta.Aberta && c.DataVencimento < hoje)
                    .CountAsync(),

                TotalPagarMes = totalPagarMes,

                TotalReceberMes = totalReceberMes,

                SaldoContas = saldoContas
            };

            return View(dashboard);
        }

        // Contas a Pagar
        public async Task<IActionResult> ContasPagar()
        {
            var contas = await _context.ContasPagar
                .Include(c => c.Fornecedor)
                .Include(c => c.CategoriaFinanceira)
                .Include(c => c.ContaBancaria)
                .OrderBy(c => c.DataVencimento)
                .ToListAsync();

            return View(contas);
        }

        [HttpGet]
        public async Task<IActionResult> CriarContaPagar()
        {
            ViewBag.Fornecedores = await _context.Fornecedores
                .Where(f => f.Ativo)
                .OrderBy(f => f.RazaoSocial)
                .ToListAsync();

            ViewBag.Categorias = await _context.CategoriasFinanceiras
                .Where(c => c.Ativa && c.Tipo == TipoCategoria.Despesa)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            ViewBag.ContasBancarias = await _context.ContasBancarias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarContaPagar(ContaPagar conta)
        {
            if (ModelState.IsValid)
            {
                conta.DataCriacao = DateTime.Now;
                conta.CriadoPorId = 1; // TODO: Pegar do usuário logado

                _context.ContasPagar.Add(conta);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Conta a pagar criada com sucesso!";
                return RedirectToAction(nameof(ContasPagar));
            }

            // Recarregar listas em caso de erro
            ViewBag.Fornecedores = await _context.Fornecedores
                .Where(f => f.Ativo)
                .OrderBy(f => f.RazaoSocial)
                .ToListAsync();

            ViewBag.Categorias = await _context.CategoriasFinanceiras
                .Where(c => c.Ativa && c.Tipo == TipoCategoria.Despesa)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            ViewBag.ContasBancarias = await _context.ContasBancarias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return View(conta);
        }

        // Contas a Receber
        public async Task<IActionResult> ContasReceber()
        {
            var contas = await _context.ContasReceber
                .Include(c => c.Cliente)
                .Include(c => c.CategoriaFinanceira)
                .Include(c => c.ContaBancaria)
                .OrderBy(c => c.DataVencimento)
                .ToListAsync();

            return View(contas);
        }

        [HttpGet]
        public async Task<IActionResult> CriarContaReceber()
        {
            ViewBag.Clientes = await _context.Clientes
                .Where(c => c.Ativo)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            ViewBag.Categorias = await _context.CategoriasFinanceiras
                .Where(c => c.Ativa && c.Tipo == TipoCategoria.Receita)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            ViewBag.ContasBancarias = await _context.ContasBancarias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarContaReceber(ContaReceber conta)
        {
            if (ModelState.IsValid)
            {
                conta.DataCriacao = DateTime.Now;
                conta.CriadoPorId = 1; // TODO: Pegar do usuário logado

                _context.ContasReceber.Add(conta);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Conta a receber criada com sucesso!";
                return RedirectToAction(nameof(ContasReceber));
            }

            // Recarregar listas em caso de erro
            ViewBag.Clientes = await _context.Clientes
                .Where(c => c.Ativo)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            ViewBag.Categorias = await _context.CategoriasFinanceiras
                .Where(c => c.Ativa && c.Tipo == TipoCategoria.Receita)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            ViewBag.ContasBancarias = await _context.ContasBancarias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return View(conta);
        }

        // Contas Bancárias
        public async Task<IActionResult> ContasBancarias()
        {
            var contas = await _context.ContasBancarias
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return View(contas);
        }

        // Fluxo de Caixa
        public async Task<IActionResult> FluxoCaixa(DateTime? dataInicio, DateTime? dataFim)
        {
            dataInicio ??= DateTime.Today.AddDays(-30);
            dataFim ??= DateTime.Today.AddDays(30);

            var movimentacoes = await _context.MovimentacoesFinanceiras
                .Include(m => m.ContaBancaria)
                .Include(m => m.CategoriaFinanceira)
                .Where(m => m.DataMovimentacao >= dataInicio && m.DataMovimentacao <= dataFim)
                .OrderBy(m => m.DataMovimentacao)
                .ToListAsync();

            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;

            return View(movimentacoes);
        }

        // API para gráficos
        [HttpGet]
        public async Task<IActionResult> DadosGrafico(string tipo, int meses = 6)
        {
            var dataInicio = DateTime.Today.AddMonths(-meses);
            
            switch (tipo)
            {
                case "receitas-despesas":
                    var movs = await _context.MovimentacoesFinanceiras
                        .Where(m => m.DataMovimentacao >= dataInicio)
                        .ToListAsync();

                    var dadosFinanceiros = movs
                        .GroupBy(m => new { m.DataMovimentacao.Year, m.DataMovimentacao.Month, m.Tipo })
                        .Select(g => new
                        {
                            Ano = g.Key.Year,
                            Mes = g.Key.Month,
                            Tipo = g.Key.Tipo.ToString(),
                            Valor = g.Sum(m => m.Valor)
                        })
                        .ToList();
                    
                    return Json(dadosFinanceiros);

                case "contas-vencer":
                    var contasPagar = await _context.ContasPagar
                        .Where(c => c.Status == StatusConta.Aberta && c.DataVencimento >= DateTime.Today)
                        .ToListAsync();

                    var contasVencer = contasPagar
                        .GroupBy(c => c.DataVencimento.Date)
                        .Select(g => new
                        {
                            Data = g.Key,
                            Valor = g.Sum(c => c.ValorOriginal + c.ValorJuros + c.ValorMulta - c.ValorDesconto)
                        })
                        .OrderBy(x => x.Data)
                        .Take(30)
                        .ToList();
                    
                    return Json(contasVencer);

                default:
                    return BadRequest("Tipo de gráfico não suportado");
            }
        }
    }
}
