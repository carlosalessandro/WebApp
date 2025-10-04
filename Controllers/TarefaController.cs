using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Text.Json;

namespace WebApp.Controllers
{
    public class TarefaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TarefaController> _logger;

        public TarefaController(ApplicationDbContext context, ILogger<TarefaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Tarefa
        public async Task<IActionResult> Index()
        {
            var tarefas = await _context.Tarefas
                .OrderBy(t => t.Ordem)
                .ThenBy(t => t.DataCriacao)
                .ToListAsync();

            return View(tarefas);
        }

        // GET: Tarefa/Kanban
        public async Task<IActionResult> Kanban()
        {
            var tarefas = await _context.Tarefas
                .OrderBy(t => t.Ordem)
                .ThenBy(t => t.DataCriacao)
                .ToListAsync();

            return View(tarefas);
        }

        // GET: Tarefa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        // GET: Tarefa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tarefa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descricao,Status,Prioridade,DataVencimento,Responsavel,EstimativaHoras,Tags,Cor")] Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Definir ordem como último item
                    var ultimaOrdem = await _context.Tarefas
                        .Where(t => t.Status == tarefa.Status)
                        .MaxAsync(t => (int?)t.Ordem) ?? 0;
                    
                    tarefa.Ordem = ultimaOrdem + 1;
                    tarefa.DataCriacao = DateTime.Now;
                    
                    _context.Add(tarefa);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Tarefa criada com sucesso!";
                    return RedirectToAction(nameof(Kanban));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar tarefa");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar a tarefa. Tente novamente.");
                }
            }
            return View(tarefa);
        }

        // GET: Tarefa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                return NotFound();
            }
            return View(tarefa);
        }

        // POST: Tarefa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Status,Prioridade,DataVencimento,Responsavel,EstimativaHoras,TempoGasto,Tags,Cor,Ordem,DataCriacao")] Tarefa tarefa)
        {
            if (id != tarefa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tarefa.DataAtualizacao = DateTime.Now;
                    _context.Update(tarefa);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Tarefa atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefaExists(tarefa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar tarefa");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar a tarefa. Tente novamente.");
                    return View(tarefa);
                }
                return RedirectToAction(nameof(Kanban));
            }
            return View(tarefa);
        }

        // GET: Tarefa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        // POST: Tarefa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var tarefa = await _context.Tarefas.FindAsync(id);
                if (tarefa != null)
                {
                    _context.Tarefas.Remove(tarefa);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Tarefa excluída com sucesso!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir tarefa");
                TempData["ErrorMessage"] = "Ocorreu um erro ao excluir a tarefa. Tente novamente.";
            }
            return RedirectToAction(nameof(Kanban));
        }

        // GET: Tarefa/TestStatus
        [HttpGet]
        public IActionResult TestStatus()
        {
            return Json(new { success = true, message = "Endpoint funcionando", timestamp = DateTime.Now });
        }

        // POST: Tarefa/UpdateStatus
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusRequest request)
        {
            try
            {
                _logger.LogInformation("Tentando atualizar tarefa {TarefaId} para status {NovoStatus}", request.TarefaId, request.NovoStatus);

                if (request.TarefaId <= 0)
                {
                    _logger.LogWarning("ID da tarefa inválido: {TarefaId}", request.TarefaId);
                    return Json(new { success = false, message = "ID da tarefa inválido" });
                }

                var tarefa = await _context.Tarefas.FindAsync(request.TarefaId);
                if (tarefa == null)
                {
                    _logger.LogWarning("Tarefa não encontrada com ID: {TarefaId}", request.TarefaId);
                    return Json(new { success = false, message = "Tarefa não encontrada" });
                }

                var statusAnterior = tarefa.Status;
                _logger.LogInformation("Status anterior: {StatusAnterior}, Novo status: {NovoStatus}", statusAnterior, request.NovoStatus);

                tarefa.Status = request.NovoStatus;
                tarefa.DataAtualizacao = DateTime.Now;

                // Reordenar tarefas se necessário
                if (statusAnterior != request.NovoStatus)
                {
                    await ReordenarTarefas(request.NovoStatus, request.NovaOrdem, tarefa.Id);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Tarefa {TarefaId} atualizada com sucesso para status {NovoStatus}", tarefa.Id, request.NovoStatus);
                
                return Json(new { success = true, message = "Tarefa atualizada com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar status da tarefa {TarefaId} para {NovoStatus}", request.TarefaId, request.NovoStatus);
                return Json(new { success = false, message = $"Erro interno do servidor: {ex.Message}" });
            }
        }

        // POST: Tarefa/UpdateOrder
        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            try
            {
                var tarefa = await _context.Tarefas.FindAsync(request.TarefaId);
                if (tarefa == null)
                {
                    return Json(new { success = false, message = "Tarefa não encontrada" });
                }

                tarefa.Ordem = request.NovaOrdem;
                tarefa.DataAtualizacao = DateTime.Now;

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar ordem da tarefa");
                return Json(new { success = false, message = "Erro interno do servidor" });
            }
        }

        private async Task ReordenarTarefas(StatusTarefa status, int novaOrdem, int tarefaId)
        {
            var tarefasNoStatus = await _context.Tarefas
                .Where(t => t.Status == status && t.Id != tarefaId)
                .OrderBy(t => t.Ordem)
                .ToListAsync();

            for (int i = 0; i < tarefasNoStatus.Count; i++)
            {
                if (i >= novaOrdem)
                {
                    tarefasNoStatus[i].Ordem = i + 2;
                }
                else
                {
                    tarefasNoStatus[i].Ordem = i + 1;
                }
            }
        }

        private bool TarefaExists(int id)
        {
            return _context.Tarefas.Any(e => e.Id == id);
        }
    }

    public class UpdateStatusRequest
    {
        public int TarefaId { get; set; }
        public StatusTarefa NovoStatus { get; set; }
        public int NovaOrdem { get; set; }
    }

    public class UpdateOrderRequest
    {
        public int TarefaId { get; set; }
        public int NovaOrdem { get; set; }
    }
}

