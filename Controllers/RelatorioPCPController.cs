using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class RelatorioPCPController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RelatorioPCPController> _logger;

        public RelatorioPCPController(ApplicationDbContext context, ILogger<RelatorioPCPController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: RelatorioPCP
        public async Task<IActionResult> Index()
        {
            var relatorios = await _context.RelatoriosPCP
                .Include(r => r.CriadoPor)
                .OrderByDescending(r => r.DataCriacao)
                .ToListAsync();

            return View(relatorios);
        }

        // GET: RelatorioPCP/Create
        public IActionResult Create()
        {
            ViewBag.TiposRelatorio = new[] { 
                "Personalizado", 
                "Ordens de Produção", 
                "Apontamentos", 
                "Recursos", 
                "Eficiência",
                "Refugo e Retrabalho",
                "Tempo de Produção"
            };
            return View();
        }

        // GET: RelatorioPCP/Builder
        public IActionResult Builder()
        {
            // Campos disponíveis para cada entidade
            ViewBag.CamposOrdemProducao = new[]
            {
                new { campo = "NumeroOrdem", label = "Número da Ordem", tipo = "text" },
                new { campo = "Produto.Nome", label = "Produto", tipo = "text" },
                new { campo = "Quantidade", label = "Quantidade", tipo = "number" },
                new { campo = "QuantidadeProduzida", label = "Quantidade Produzida", tipo = "number" },
                new { campo = "Status", label = "Status", tipo = "select" },
                new { campo = "Prioridade", label = "Prioridade", tipo = "number" },
                new { campo = "DataInicioPrevista", label = "Data Início Prevista", tipo = "date" },
                new { campo = "DataFimPrevista", label = "Data Fim Prevista", tipo = "date" },
                new { campo = "DataInicioReal", label = "Data Início Real", tipo = "datetime" },
                new { campo = "DataFimReal", label = "Data Fim Real", tipo = "datetime" },
                new { campo = "DataCriacao", label = "Data de Criação", tipo = "date" }
            };

            ViewBag.CamposApontamento = new[]
            {
                new { campo = "OrdemProducao.NumeroOrdem", label = "Número da Ordem", tipo = "text" },
                new { campo = "OrdemProducao.Produto.Nome", label = "Produto", tipo = "text" },
                new { campo = "QuantidadeProduzida", label = "Quantidade Produzida", tipo = "number" },
                new { campo = "QuantidadeRefugo", label = "Quantidade Refugo", tipo = "number" },
                new { campo = "QuantidadeRetrabalho", label = "Quantidade Retrabalho", tipo = "number" },
                new { campo = "DataHoraApontamento", label = "Data/Hora Apontamento", tipo = "datetime" },
                new { campo = "Operador.Name", label = "Operador", tipo = "text" },
                new { campo = "TempoProducaoMinutos", label = "Tempo Produção (min)", tipo = "number" },
                new { campo = "TempoSetupMinutos", label = "Tempo Setup (min)", tipo = "number" },
                new { campo = "TempoParadoMinutos", label = "Tempo Parado (min)", tipo = "number" },
                new { campo = "MotivoParada", label = "Motivo Parada", tipo = "text" }
            };

            ViewBag.CamposRecurso = new[]
            {
                new { campo = "Codigo", label = "Código", tipo = "text" },
                new { campo = "Nome", label = "Nome", tipo = "text" },
                new { campo = "Tipo", label = "Tipo", tipo = "select" },
                new { campo = "CapacidadePorHora", label = "Capacidade/Hora", tipo = "number" },
                new { campo = "CustoPorHora", label = "Custo/Hora", tipo = "currency" },
                new { campo = "Disponivel", label = "Disponível", tipo = "boolean" },
                new { campo = "EmManutencao", label = "Em Manutenção", tipo = "boolean" },
                new { campo = "Localizacao", label = "Localização", tipo = "text" }
            };

            return View();
        }

        // POST: RelatorioPCP/SaveBuilder
        [HttpPost]
        public async Task<IActionResult> SaveBuilder([FromBody] RelatorioBuilderDto dto)
        {
            try
            {
                var relatorio = new RelatorioPCP
                {
                    Nome = dto.Nome,
                    Descricao = dto.Descricao,
                    TipoRelatorio = dto.TipoRelatorio,
                    ConfiguracaoJson = JsonSerializer.Serialize(dto.Configuracao),
                    CamposSelecionados = dto.CamposSelecionados != null 
                        ? string.Join(",", dto.CamposSelecionados) 
                        : null,
                    Filtros = dto.Filtros != null 
                        ? JsonSerializer.Serialize(dto.Filtros) 
                        : null,
                    Ordenacao = dto.Ordenacao,
                    Agrupamento = dto.Agrupamento,
                    Publico = dto.Publico,
                    CriadoPorId = 1, // TODO: Pegar do usuário logado
                    DataCriacao = DateTime.Now
                };

                _context.RelatoriosPCP.Add(relatorio);
                await _context.SaveChangesAsync();

                return Json(new { success = true, id = relatorio.Id, message = "Relatório salvo com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar relatório");
                return Json(new { success = false, message = $"Erro ao salvar: {ex.Message}" });
            }
        }

        // GET: RelatorioPCP/Execute/5
        public async Task<IActionResult> Execute(int? id)
        {
            if (id == null) return NotFound();

            var relatorio = await _context.RelatoriosPCP.FindAsync(id);
            if (relatorio == null) return NotFound();

            // Processar a configuração do relatório
            var campos = relatorio.CamposSelecionados?.Split(',').ToList() ?? new List<string>();
            
            // Buscar dados conforme o tipo de relatório
            dynamic dados;
            
            switch (relatorio.TipoRelatorio)
            {
                case "Ordens de Produção":
                case "Personalizado":
                    dados = await _context.OrdensProducao
                        .Include(o => o.Produto)
                        .Include(o => o.CriadoPor)
                        .ToListAsync();
                    break;
                    
                case "Apontamentos":
                    dados = await _context.ApontamentosProducao
                        .Include(a => a.OrdemProducao)
                            .ThenInclude(o => o.Produto)
                        .Include(a => a.Operador)
                        .ToListAsync();
                    break;
                    
                case "Recursos":
                    dados = await _context.Recursos.ToListAsync();
                    break;
                    
                default:
                    dados = new List<object>();
                    break;
            }

            ViewBag.Relatorio = relatorio;
            ViewBag.Campos = campos;
            
            return View(dados);
        }

        // GET: RelatorioPCP/GetData
        [HttpGet]
        public async Task<IActionResult> GetData(string tipo)
        {
            try
            {
                object dados;
                
                switch (tipo)
                {
                    case "ordens":
                        dados = await _context.OrdensProducao
                            .Include(o => o.Produto)
                            .Select(o => new
                            {
                                o.Id,
                                o.NumeroOrdem,
                                Produto = o.Produto != null ? o.Produto.Nome : "",
                                o.Quantidade,
                                o.QuantidadeProduzida,
                                o.Status,
                                o.Prioridade,
                                o.DataInicioPrevista,
                                o.DataFimPrevista,
                                o.DataInicioReal,
                                o.DataFimReal,
                                o.DataCriacao
                            })
                            .ToListAsync();
                        break;
                        
                    case "apontamentos":
                        dados = await _context.ApontamentosProducao
                            .Include(a => a.OrdemProducao)
                                .ThenInclude(o => o.Produto)
                            .Include(a => a.Operador)
                            .Select(a => new
                            {
                                a.Id,
                                NumeroOrdem = a.OrdemProducao != null ? a.OrdemProducao.NumeroOrdem : "",
                                Produto = a.OrdemProducao != null && a.OrdemProducao.Produto != null 
                                    ? a.OrdemProducao.Produto.Nome : "",
                                a.QuantidadeProduzida,
                                a.QuantidadeRefugo,
                                a.QuantidadeRetrabalho,
                                a.DataHoraApontamento,
                                Operador = a.Operador != null ? a.Operador.Name : "",
                                a.TempoProducaoMinutos,
                                a.TempoSetupMinutos,
                                a.TempoParadoMinutos,
                                a.MotivoParada
                            })
                            .ToListAsync();
                        break;
                        
                    case "recursos":
                        dados = await _context.Recursos
                            .Select(r => new
                            {
                                r.Id,
                                r.Codigo,
                                r.Nome,
                                r.Tipo,
                                r.CapacidadePorHora,
                                r.CustoPorHora,
                                r.Disponivel,
                                r.EmManutencao,
                                r.Localizacao
                            })
                            .ToListAsync();
                        break;
                        
                    default:
                        return Json(new { success = false, message = "Tipo de dados inválido" });
                }

                return Json(new { success = true, data = dados });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // DELETE: RelatorioPCP/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var relatorio = await _context.RelatoriosPCP.FindAsync(id);
                if (relatorio == null)
                    return Json(new { success = false, message = "Relatório não encontrado" });

                _context.RelatoriosPCP.Remove(relatorio);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Relatório excluído com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir relatório");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    // DTOs
    public class RelatorioBuilderDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string TipoRelatorio { get; set; } = "Personalizado";
        public object? Configuracao { get; set; }
        public List<string>? CamposSelecionados { get; set; }
        public object? Filtros { get; set; }
        public string? Ordenacao { get; set; }
        public string? Agrupamento { get; set; }
        public bool Publico { get; set; }
    }
}
