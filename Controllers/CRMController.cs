using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Models.CRM;
using System.Text.Json;

namespace WebApp.Controllers
{
    [Authorize]
    public class CRMController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CRMController> _logger;

        public CRMController(ApplicationDbContext context, ILogger<CRMController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Dashboard CRM
        public async Task<IActionResult> Index()
        {
            try
            {
                var hoje = DateTime.Today;
                var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
                var fimMes = inicioMes.AddMonths(1).AddDays(-1);

                // Estatísticas de Leads
                var totalLeads = await _context.Leads.CountAsync();
                var leadsNovos = await _context.Leads.CountAsync(l => l.Status == StatusLead.Novo);
                var leadsQualificados = await _context.Leads.CountAsync(l => l.Status == StatusLead.Qualificado);
                var leadsConvertidos = await _context.Leads.CountAsync(l => l.Status == StatusLead.Fechado);

                // Estatísticas de Oportunidades
                var totalOportunidades = await _context.Oportunidades.CountAsync();
                var oportunidadesAbertas = await _context.Oportunidades.CountAsync(o => 
                    o.Status == StatusOportunidade.Identificacao || 
                    o.Status == StatusOportunidade.Qualificacao ||
                    o.Status == StatusOportunidade.Analise ||
                    o.Status == StatusOportunidade.Proposta ||
                    o.Status == StatusOportunidade.Negociacao);

                var valorPipeline = await _context.Oportunidades
                    .Where(o => o.Status != StatusOportunidade.FechadaGanha && 
                               o.Status != StatusOportunidade.FechadaPerdida &&
                               o.Status != StatusOportunidade.Cancelada)
                    .SumAsync(o => o.ValorEsperado);

                // Estatísticas de Campanhas
                var campanhasAtivas = await _context.CampanhasMarketing
                    .CountAsync(c => c.Status == StatusCampanha.Ativa);

                var leadsEsteMes = await _context.Leads
                    .CountAsync(l => l.DataCriacao >= inicioMes && l.DataCriacao <= fimMes);

                // Taxa de conversão
                var taxaConversao = totalLeads > 0 ? Math.Round((double)leadsConvertidos / totalLeads * 100, 2) : 0;

                ViewBag.TotalLeads = totalLeads;
                ViewBag.LeadsNovos = leadsNovos;
                ViewBag.LeadsQualificados = leadsQualificados;
                ViewBag.LeadsConvertidos = leadsConvertidos;
                ViewBag.TotalOportunidades = totalOportunidades;
                ViewBag.OportunidadesAbertas = oportunidadesAbertas;
                ViewBag.ValorPipeline = valorPipeline;
                ViewBag.CampanhasAtivas = campanhasAtivas;
                ViewBag.LeadsEsteMes = leadsEsteMes;
                ViewBag.TaxaConversao = taxaConversao;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar dashboard CRM");
                return View("Error");
            }
        }

        // Leads
        public async Task<IActionResult> Leads()
        {
            var leads = await _context.Leads
                .Include(l => l.Origem)
                .Include(l => l.Responsavel)
                .Include(l => l.Campanha)
                .OrderByDescending(l => l.DataCriacao)
                .ToListAsync();

            return View(leads);
        }

        public async Task<IActionResult> LeadDetails(int? id)
        {
            if (id == null) return NotFound();

            var lead = await _context.Leads
                .Include(l => l.Origem)
                .Include(l => l.Responsavel)
                .Include(l => l.Campanha)
                .Include(l => l.Atividades.OrderByDescending(a => a.DataCriacao))
                .Include(l => l.Oportunidades.OrderByDescending(o => o.DataCriacao))
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lead == null) return NotFound();

            return View(lead);
        }

        public IActionResult LeadCreate()
        {
            ViewBag.Origens = _context.OrigensLeads.Where(o => o.Ativo).ToList();
            ViewBag.Campanhas = _context.CampanhasMarketing.Where(c => c.Status == StatusCampanha.Ativa).ToList();
            ViewBag.Usuarios = _context.Users.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeadCreate(Lead lead)
        {
            if (ModelState.IsValid)
            {
                lead.DataCriacao = DateTime.Now;
                _context.Leads.Add(lead);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Lead {lead.Id} criado com sucesso");
                return RedirectToAction(nameof(Leads));
            }

            ViewBag.Origens = _context.OrigensLeads.Where(o => o.Ativo).ToList();
            ViewBag.Campanhas = _context.CampanhasMarketing.Where(c => c.Status == StatusCampanha.Ativa).ToList();
            ViewBag.Usuarios = _context.Users.ToList();
            return View(lead);
        }

        // Oportunidades
        public async Task<IActionResult> Oportunidades()
        {
            var oportunidades = await _context.Oportunidades
                .Include(o => o.Lead)
                .Include(o => o.Responsavel)
                .OrderByDescending(o => o.DataCriacao)
                .ToListAsync();

            return View(oportunidades);
        }

        public async Task<IActionResult> OportunidadeDetails(int? id)
        {
            if (id == null) return NotFound();

            var oportunidade = await _context.Oportunidades
                .Include(o => o.Lead)
                .Include(o => o.Responsavel)
                .Include(o => o.Atividades.OrderByDescending(a => a.DataCriacao))
                .Include(o => o.Propostas.OrderByDescending(p => p.DataEmissao))
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oportunidade == null) return NotFound();

            return View(oportunidade);
        }

        // Campanhas
        public async Task<IActionResult> Campanhas()
        {
            var campanhas = await _context.CampanhasMarketing
                .Include(c => c.CriadoPor)
                .Include(c => c.Atividades)
                .Include(c => c.Leads)
                .OrderByDescending(c => c.DataCriacao)
                .ToListAsync();

            return View(campanhas);
        }

        public IActionResult CampanhaCreate()
        {
            ViewBag.Usuarios = _context.Users.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CampanhaCreate(CampanhaMarketing campanha)
        {
            if (ModelState.IsValid)
            {
                campanha.DataCriacao = DateTime.Now;
                campanha.Status = StatusCampanha.Rascunho;
                _context.CampanhasMarketing.Add(campanha);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Campanha {campanha.Id} criada com sucesso");
                return RedirectToAction(nameof(Campanhas));
            }

            ViewBag.Usuarios = _context.Users.ToList();
            return View(campanha);
        }

        // API para gráficos
        [HttpGet]
        public async Task<IActionResult> GetLeadsPorOrigem()
        {
            var dados = await _context.Leads
                .GroupBy(l => l.Origem.Nome)
                .Select(g => new { Origem = g.Key, Quantidade = g.Count() })
                .ToListAsync();

            return Json(dados);
        }

        [HttpGet]
        public async Task<IActionResult> GetOportunidadesPorStatus()
        {
            var dados = await _context.Oportunidades
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key.ToString(), Quantidade = g.Count() })
                .ToListAsync();

            return Json(dados);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeadsUltimos30Dias()
        {
            var dataInicio = DateTime.Today.AddDays(-30);
            var dados = await _context.Leads
                .Where(l => l.DataCriacao >= dataInicio)
                .GroupBy(l => new { Ano = l.DataCriacao.Year, Mes = l.DataCriacao.Month, Dia = l.DataCriacao.Day })
                .OrderBy(g => g.Key.Ano).ThenBy(g => g.Key.Mes).ThenBy(g => g.Key.Dia)
                .Select(g => new { Data = $"{g.Key.Dia}/{g.Key.Mes}", Quantidade = g.Count() })
                .ToListAsync();

            return Json(dados);
        }
    }
}
