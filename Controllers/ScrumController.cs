using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ScrumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScrumController> _logger;

        public ScrumController(ApplicationDbContext context, ILogger<ScrumController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Scrum
        public async Task<IActionResult> Index()
        {
            var sprints = await _context.Sprints
                .Include(s => s.UserStories)
                .OrderByDescending(s => s.DataCriacao)
                .ToListAsync();

            return View(sprints);
        }

        // GET: Scrum/Backlog
        public async Task<IActionResult> Backlog()
        {
            var userStories = await _context.UserStories
                .Where(us => us.SprintId == null || us.Status == StatusUserStory.Backlog)
                .OrderByDescending(us => us.Prioridade)
                .ThenByDescending(us => us.DataCriacao)
                .ToListAsync();

            ViewBag.Sprints = await _context.Sprints
                .Where(s => s.Status == StatusSprint.Planejamento || s.Status == StatusSprint.Ativo)
                .OrderBy(s => s.DataInicio)
                .ToListAsync();

            return View(userStories);
        }

        // GET: Scrum/SprintBoard/5
        public async Task<IActionResult> SprintBoard(int id)
        {
            var sprint = await _context.Sprints
                .Include(s => s.UserStories)
                .ThenInclude(us => us.Tasks)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sprint == null)
            {
                return NotFound();
            }

            return View(sprint);
        }

        // GET: Scrum/Planning
        public async Task<IActionResult> Planning()
        {
            var backlogItems = await _context.UserStories
                .Where(us => us.SprintId == null)
                .OrderByDescending(us => us.Prioridade)
                .ThenByDescending(us => us.DataCriacao)
                .ToListAsync();

            var sprintsAtivos = await _context.Sprints
                .Where(s => s.Status == StatusSprint.Planejamento || s.Status == StatusSprint.Ativo)
                .OrderBy(s => s.DataInicio)
                .ToListAsync();

            ViewBag.BacklogItems = backlogItems;
            ViewBag.SprintsAtivos = sprintsAtivos;

            return View();
        }

        // GET: Scrum/CreateSprint
        public IActionResult CreateSprint()
        {
            var sprint = new Sprint
            {
                DataInicio = DateTime.Today.AddDays(1),
                DataFim = DateTime.Today.AddDays(14) // Sprint de 2 semanas por padrão
            };

            return View(sprint);
        }

        // POST: Scrum/CreateSprint
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSprint(Sprint sprint)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Sprints.Add(sprint);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Sprint criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar sprint");
                    TempData["Error"] = "Erro ao criar sprint. Tente novamente.";
                }
            }

            return View(sprint);
        }

        // GET: Scrum/CreateUserStory
        public IActionResult CreateUserStory()
        {
            ViewBag.Sprints = _context.Sprints
                .Where(s => s.Status == StatusSprint.Planejamento || s.Status == StatusSprint.Ativo)
                .OrderBy(s => s.DataInicio)
                .ToList();

            return View();
        }

        // POST: Scrum/CreateUserStory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserStory(UserStory userStory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserStories.Add(userStory);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "User Story criada com sucesso!";
                    return RedirectToAction(nameof(Backlog));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar user story");
                    TempData["Error"] = "Erro ao criar user story. Tente novamente.";
                }
            }

            ViewBag.Sprints = _context.Sprints
                .Where(s => s.Status == StatusSprint.Planejamento || s.Status == StatusSprint.Ativo)
                .OrderBy(s => s.DataInicio)
                .ToList();

            return View(userStory);
        }

        // POST: Scrum/MoveToSprint
        [HttpPost]
        public async Task<IActionResult> MoveToSprint(int userStoryId, int sprintId)
        {
            try
            {
                var userStory = await _context.UserStories.FindAsync(userStoryId);
                if (userStory == null)
                {
                    return Json(new { success = false, message = "User Story não encontrada" });
                }

                userStory.SprintId = sprintId;
                userStory.Status = StatusUserStory.SprintBacklog;
                userStory.DataAtualizacao = DateTime.Now;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "User Story movida para o sprint com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao mover user story para sprint");
                return Json(new { success = false, message = "Erro ao mover user story" });
            }
        }

        // POST: Scrum/UpdateUserStoryStatus
        [HttpPost]
        public async Task<IActionResult> UpdateUserStoryStatus(int userStoryId, StatusUserStory novoStatus)
        {
            try
            {
                var userStory = await _context.UserStories.FindAsync(userStoryId);
                if (userStory == null)
                {
                    return Json(new { success = false, message = "User Story não encontrada" });
                }

                userStory.Status = novoStatus;
                userStory.DataAtualizacao = DateTime.Now;

                if (novoStatus == StatusUserStory.Concluida)
                {
                    userStory.DataConclusao = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Status atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar status da user story");
                return Json(new { success = false, message = "Erro ao atualizar status" });
            }
        }

        // GET: Scrum/SprintReview/5
        public async Task<IActionResult> SprintReview(int id)
        {
            var sprint = await _context.Sprints
                .Include(s => s.UserStories)
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sprint == null)
            {
                return NotFound();
            }

            return View(sprint);
        }

        // POST: Scrum/CreateReview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview(SprintReview review)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.SprintReviews.Add(review);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Review criada com sucesso!";
                    return RedirectToAction(nameof(SprintReview), new { id = review.SprintId });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar review");
                    TempData["Error"] = "Erro ao criar review. Tente novamente.";
                }
            }

            return RedirectToAction(nameof(SprintReview), new { id = review.SprintId });
        }

        // GET: Scrum/BurndownChart/5
        public async Task<IActionResult> BurndownChart(int id)
        {
            var sprint = await _context.Sprints
                .Include(s => s.UserStories)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sprint == null)
            {
                return NotFound();
            }

            // Calcular dados do burndown chart
            var totalStoryPoints = sprint.StoryPointsTotal;
            var diasSprint = sprint.DuracaoEmDias;
            
            // Dados ideais (linha reta)
            var dadosIdeais = new List<object>();
            for (int i = 0; i <= diasSprint; i++)
            {
                var pontos = totalStoryPoints - (totalStoryPoints * i / diasSprint);
                dadosIdeais.Add(new { dia = i, pontos = pontos });
            }

            ViewBag.DadosIdeais = dadosIdeais;
            ViewBag.TotalStoryPoints = totalStoryPoints;

            return View(sprint);
        }
    }
}
