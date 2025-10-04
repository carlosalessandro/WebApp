using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Text.Json;

namespace WebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Dashboard
        public IActionResult Index()
        {
            return View();
        }

        // GET: Dashboard/Clientes
        public IActionResult Clientes()
        {
            return View();
        }

        // GET: Dashboard/Tarefas
        public IActionResult Tarefas()
        {
            return View();
        }

        // GET: Dashboard/Usuarios
        public IActionResult Usuarios()
        {
            return View();
        }

        // API Endpoints para dados dos gráficos

        // GET: Dashboard/GetClientesPorMes
        [HttpGet]
        public async Task<IActionResult> GetClientesPorMes()
        {
            try
            {
                var clientesPorMes = await _context.Clientes
                    .GroupBy(c => new { c.DataCadastro.Year, c.DataCadastro.Month })
                    .Select(g => new
                    {
                        Mes = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Total = g.Count()
                    })
                    .OrderBy(x => x.Mes)
                    .ToListAsync();

                return Json(clientesPorMes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados de clientes por mês");
                return Json(new List<object>());
            }
        }

        // GET: Dashboard/GetTarefasPorStatus
        [HttpGet]
        public async Task<IActionResult> GetTarefasPorStatus()
        {
            try
            {
                var tarefasPorStatus = await _context.Tarefas
                    .GroupBy(t => t.Status)
                    .Select(g => new
                    {
                        Status = g.Key.ToString(),
                        Total = g.Count()
                    })
                    .ToListAsync();

                return Json(tarefasPorStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados de tarefas por status");
                return Json(new List<object>());
            }
        }

        // GET: Dashboard/GetTarefasPorPrioridade
        [HttpGet]
        public async Task<IActionResult> GetTarefasPorPrioridade()
        {
            try
            {
                var tarefasPorPrioridade = await _context.Tarefas
                    .GroupBy(t => t.Prioridade)
                    .Select(g => new
                    {
                        Prioridade = g.Key.ToString(),
                        Total = g.Count()
                    })
                    .ToListAsync();

                return Json(tarefasPorPrioridade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados de tarefas por prioridade");
                return Json(new List<object>());
            }
        }

        // GET: Dashboard/GetTarefasPorResponsavel
        [HttpGet]
        public async Task<IActionResult> GetTarefasPorResponsavel()
        {
            try
            {
                var tarefasPorResponsavel = await _context.Tarefas
                    .Where(t => !string.IsNullOrEmpty(t.Responsavel))
                    .GroupBy(t => t.Responsavel)
                    .Select(g => new
                    {
                        Responsavel = g.Key,
                        Total = g.Count()
                    })
                    .OrderByDescending(x => x.Total)
                    .Take(10)
                    .ToListAsync();

                return Json(tarefasPorResponsavel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados de tarefas por responsável");
                return Json(new List<object>());
            }
        }

        // GET: Dashboard/GetTarefasConcluidasPorMes
        [HttpGet]
        public async Task<IActionResult> GetTarefasConcluidasPorMes()
        {
            try
            {
                var tarefasConcluidas = await _context.Tarefas
                    .Where(t => t.Status == StatusTarefa.Done && t.DataAtualizacao.HasValue)
                    .GroupBy(t => new { t.DataAtualizacao!.Value.Year, t.DataAtualizacao!.Value.Month })
                    .Select(g => new
                    {
                        Mes = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Total = g.Count()
                    })
                    .OrderBy(x => x.Mes)
                    .ToListAsync();

                return Json(tarefasConcluidas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados de tarefas concluídas por mês");
                return Json(new List<object>());
            }
        }

        // GET: Dashboard/GetEstatisticasGerais
        [HttpGet]
        public async Task<IActionResult> GetEstatisticasGerais()
        {
            try
            {
                var totalClientes = await _context.Clientes.CountAsync();
                var totalTarefas = await _context.Tarefas.CountAsync();
                var tarefasConcluidas = await _context.Tarefas.CountAsync(t => t.Status == StatusTarefa.Done);
                var tarefasEmProgresso = await _context.Tarefas.CountAsync(t => t.Status == StatusTarefa.InProgress);
                var totalUsuarios = await _context.Users.CountAsync();

                var estatisticas = new
                {
                    TotalClientes = totalClientes,
                    TotalTarefas = totalTarefas,
                    TarefasConcluidas = tarefasConcluidas,
                    TarefasEmProgresso = tarefasEmProgresso,
                    TotalUsuarios = totalUsuarios,
                    TaxaConclusao = totalTarefas > 0 ? Math.Round((double)tarefasConcluidas / totalTarefas * 100, 2) : 0
                };

                return Json(estatisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar estatísticas gerais");
                return Json(new { });
            }
        }

        // GET: Dashboard/GetTarefasAtrasadas
        [HttpGet]
        public async Task<IActionResult> GetTarefasAtrasadas()
        {
            try
            {
                var tarefasAtrasadas = await _context.Tarefas
                    .Where(t => t.DataVencimento.HasValue && 
                               t.DataVencimento.Value < DateTime.Now && 
                               t.Status != StatusTarefa.Done)
                    .Select(t => new
                    {
                        Id = t.Id,
                        Titulo = t.Titulo,
                        DataVencimento = t.DataVencimento!.Value,
                        Status = t.Status.ToString(),
                        Responsavel = t.Responsavel,
                        DiasAtraso = (DateTime.Now - t.DataVencimento!.Value).Days
                    })
                    .OrderByDescending(t => t.DiasAtraso)
                    .ToListAsync();

                return Json(tarefasAtrasadas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tarefas atrasadas");
                return Json(new List<object>());
            }
        }

        // GET: Dashboard/GetProdutividadePorResponsavel
        [HttpGet]
        public async Task<IActionResult> GetProdutividadePorResponsavel()
        {
            try
            {
                var produtividade = await _context.Tarefas
                    .Where(t => !string.IsNullOrEmpty(t.Responsavel))
                    .GroupBy(t => t.Responsavel)
                    .Select(g => new
                    {
                        Responsavel = g.Key,
                        TotalTarefas = g.Count(),
                        TarefasConcluidas = g.Count(t => t.Status == StatusTarefa.Done),
                        TarefasEmProgresso = g.Count(t => t.Status == StatusTarefa.InProgress),
                        TaxaConclusao = g.Count() > 0 ? Math.Round((double)g.Count(t => t.Status == StatusTarefa.Done) / g.Count() * 100, 2) : 0
                    })
                    .OrderByDescending(x => x.TaxaConclusao)
                    .ToListAsync();

                return Json(produtividade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar produtividade por responsável");
                return Json(new List<object>());
            }
        }

        // GET: Dashboard/GetTempoGastoPorTarefa
        [HttpGet]
        public async Task<IActionResult> GetTempoGastoPorTarefa()
        {
            try
            {
                var tempoGasto = await _context.Tarefas
                    .Where(t => t.TempoGasto.HasValue && t.TempoGasto > 0)
                    .Select(t => new
                    {
                        Titulo = t.Titulo,
                        TempoGasto = t.TempoGasto ?? 0,
                        Estimativa = t.EstimativaHoras ?? 0,
                        Diferenca = (t.TempoGasto ?? 0) - (t.EstimativaHoras ?? 0),
                        Responsavel = t.Responsavel
                    })
                    .OrderByDescending(t => t.TempoGasto)
                    .Take(20)
                    .ToListAsync();

                return Json(tempoGasto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tempo gasto por tarefa");
                return Json(new List<object>());
            }
        }
    }
}
