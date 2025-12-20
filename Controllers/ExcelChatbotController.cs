using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Text.Json;

namespace WebApp.Controllers
{
    [Authorize]
    public class ExcelChatbotController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExcelChatbotController> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExcelChatbotController(ApplicationDbContext context, ILogger<ExcelChatbotController> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var sessions = _context.ExcelChatbotSessions
                .Where(s => s.UserId == userId && s.IsActive)
                .OrderByDescending(s => s.LastActivity)
                .ToList();

            ViewBag.Sessions = sessions;
            return View();
        }

        [HttpGet]
        public IActionResult CreateSession()
        {
            var sessionId = Guid.NewGuid().ToString();
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var session = new ExcelChatbotSession
            {
                SessionId = sessionId,
                UserId = userId,
                CreatedAt = DateTime.Now,
                LastActivity = DateTime.Now,
                IsActive = true
            };

            _context.ExcelChatbotSessions.Add(session);
            _context.SaveChanges();

            return Json(new { success = true, sessionId = session.SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file, string sessionId)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "Nenhum arquivo selecionado" });
            }

            if (!Path.GetExtension(file.FileName).ToLower().Contains("xls") && 
                !Path.GetExtension(file.FileName).ToLower().Contains("xlsx"))
            {
                return Json(new { success = false, message = "Por favor, selecione um arquivo Excel válido" });
            }

            try
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "excel");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Atualizar sessão com informações do arquivo
                var session = await _context.ExcelChatbotSessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session != null)
                {
                    session.FileName = file.FileName;
                    session.FilePath = filePath;
                    session.LastActivity = DateTime.Now;
                    await _context.SaveChangesAsync();
                }

                // TODO: Processar arquivo Excel e extrair informações
                var fileInfo = new
                {
                    fileName = file.FileName,
                    fileSize = file.Length,
                    uploadedAt = DateTime.Now,
                    sessionId = sessionId
                };

                return Json(new { 
                    success = true, 
                    message = "Arquivo Excel carregado com sucesso!",
                    fileInfo = fileInfo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer upload do arquivo Excel");
                return Json(new { success = false, message = "Erro ao processar o arquivo: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSessionData(string sessionId)
        {
            var session = await _context.ExcelChatbotSessions
                .Include(s => s.Messages)
                .Include(s => s.Operations)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session == null)
            {
                return Json(new { success = false, message = "Sessão não encontrada" });
            }

            var messages = session.Messages
                .OrderBy(m => m.Timestamp)
                .Select(m => new
                {
                    id = m.Id,
                    type = m.Type.ToString().ToLower(),
                    content = m.Content,
                    timestamp = m.Timestamp.ToString("HH:mm:ss")
                })
                .ToList();

            var operations = session.Operations
                .OrderByDescending(o => o.ExecutedAt)
                .Take(10)
                .Select(o => new
                {
                    id = o.Id,
                    operationType = o.OperationType,
                    description = o.Description,
                    success = o.Success,
                    executedAt = o.ExecutedAt.ToString("dd/MM/yyyy HH:mm:ss")
                })
                .ToList();

            return Json(new
            {
                success = true,
                session = new
                {
                    id = session.Id,
                    sessionId = session.SessionId,
                    fileName = session.FileName,
                    createdAt = session.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                    lastActivity = session.LastActivity.ToString("dd/MM/yyyy HH:mm:ss")
                },
                messages = messages,
                operations = operations
            });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string sessionId, string message)
        {
            try
            {
                var session = await _context.ExcelChatbotSessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session == null)
                {
                    return Json(new { success = false, message = "Sessão não encontrada" });
                }

                // Salvar mensagem do usuário
                var userMessage = new ExcelChatbotMessage
                {
                    SessionId = session.Id,
                    Type = MessageType.User,
                    Content = message,
                    Timestamp = DateTime.Now
                };

                _context.ExcelChatbotMessages.Add(userMessage);

                // TODO: Processar mensagem com IA e gerar resposta
                var botResponse = await ProcessMessageWithAI(session, message);

                // Salvar resposta do bot
                var botMessage = new ExcelChatbotMessage
                {
                    SessionId = session.Id,
                    Type = MessageType.Bot,
                    Content = botResponse,
                    Timestamp = DateTime.Now
                };

                _context.ExcelChatbotMessages.Add(botMessage);

                // Atualizar última atividade da sessão
                session.LastActivity = DateTime.Now;

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    userMessage = new
                    {
                        id = userMessage.Id,
                        type = "user",
                        content = message,
                        timestamp = userMessage.Timestamp.ToString("HH:mm:ss")
                    },
                    botMessage = new
                    {
                        id = botMessage.Id,
                        type = "bot",
                        content = botResponse,
                        timestamp = botMessage.Timestamp.ToString("HH:mm:ss")
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem do chatbot");
                return Json(new { success = false, message = "Erro ao processar mensagem: " + ex.Message });
            }
        }

        private Task<string> ProcessMessageWithAI(ExcelChatbotSession session, string message)
        {
            // TODO: Implementar integração com IA
            // Por enquanto, retornar uma resposta simulada
            
            var responses = new[]
            {
                "Entendi! Posso ajudar você a analisar seus dados Excel. O que você gostaria de fazer?",
                "Ótima pergunta! Vou processar sua solicitação sobre o arquivo Excel.",
                "Recebi sua mensagem. Estou analisando como posso ajudar com sua planilha.",
                "Entendido! Posso realizar diversas operações com seu arquivo Excel. Qual seria sua necessidade?"
            };

            var random = new Random();
            var response = responses[random.Next(responses.Length)];

            // Adicionar contexto se houver arquivo
            if (!string.IsNullOrEmpty(session.FileName))
            {
                response += $"\n\nVejo que você carregou o arquivo '{session.FileName}'.";
            }

            return Task.FromResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteOperation(string sessionId, string operationType, string parameters)
        {
            try
            {
                var session = await _context.ExcelChatbotSessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session == null)
                {
                    return Json(new { success = false, message = "Sessão não encontrada" });
                }

                // TODO: Implementar lógica de execução de operações Excel
                var operation = new ExcelChatbotOperation
                {
                    SessionId = session.Id,
                    OperationType = operationType,
                    Description = $"Executando operação: {operationType}",
                    Parameters = parameters,
                    Success = true,
                    ExecutedAt = DateTime.Now
                };

                _context.ExcelChatbotOperations.Add(operation);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    operation = new
                    {
                        id = operation.Id,
                        operationType = operation.OperationType,
                        description = operation.Description,
                        success = operation.Success,
                        executedAt = operation.ExecutedAt.ToString("dd/MM/yyyy HH:mm:ss")
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar operação Excel");
                return Json(new { success = false, message = "Erro ao executar operação: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSession(string sessionId)
        {
            try
            {
                var session = await _context.ExcelChatbotSessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session == null)
                {
                    return Json(new { success = false, message = "Sessão não encontrada" });
                }

                session.IsActive = false;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Sessão excluída com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir sessão");
                return Json(new { success = false, message = "Erro ao excluir sessão: " + ex.Message });
            }
        }
    }
}
