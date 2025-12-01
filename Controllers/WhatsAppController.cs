using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize]
    public class WhatsAppController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWhatsAppService _whatsAppService;

        public WhatsAppController(ApplicationDbContext context, IWhatsAppService whatsAppService)
        {
            _context = context;
            _whatsAppService = whatsAppService;
        }

        // GET: WhatsApp
        public async Task<IActionResult> Index()
        {
            return View(await _context.WhatsAppIntegracoes.ToListAsync());
        }

        // GET: WhatsApp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var whatsAppIntegracao = await _context.WhatsAppIntegracoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (whatsAppIntegracao == null)
            {
                return NotFound();
            }

            return View(whatsAppIntegracao);
        }

        // GET: WhatsApp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WhatsApp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TokenAcesso,NumeroTelefone,BusinessAccountId,Ativo,WebhookUrl,WebhookSecret")] WhatsAppIntegracao whatsAppIntegracao)
        {
            if (ModelState.IsValid)
            {
                whatsAppIntegracao.DataCriacao = DateTime.Now;
                _context.Add(whatsAppIntegracao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(whatsAppIntegracao);
        }

        // GET: WhatsApp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var whatsAppIntegracao = await _context.WhatsAppIntegracoes.FindAsync(id);
            if (whatsAppIntegracao == null)
            {
                return NotFound();
            }
            return View(whatsAppIntegracao);
        }

        // POST: WhatsApp/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TokenAcesso,NumeroTelefone,BusinessAccountId,Ativo,WebhookUrl,WebhookSecret")] WhatsAppIntegracao whatsAppIntegracao)
        {
            if (id != whatsAppIntegracao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingIntegracao = await _context.WhatsAppIntegracoes.FindAsync(id);
                    existingIntegracao.TokenAcesso = whatsAppIntegracao.TokenAcesso;
                    existingIntegracao.NumeroTelefone = whatsAppIntegracao.NumeroTelefone;
                    existingIntegracao.BusinessAccountId = whatsAppIntegracao.BusinessAccountId;
                    existingIntegracao.Ativo = whatsAppIntegracao.Ativo;
                    existingIntegracao.WebhookUrl = whatsAppIntegracao.WebhookUrl;
                    existingIntegracao.WebhookSecret = whatsAppIntegracao.WebhookSecret;
                    existingIntegracao.UltimaAtualizacao = DateTime.Now;
                    
                    _context.Update(existingIntegracao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WhatsAppIntegracaoExists(whatsAppIntegracao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(whatsAppIntegracao);
        }

        // GET: WhatsApp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var whatsAppIntegracao = await _context.WhatsAppIntegracoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (whatsAppIntegracao == null)
            {
                return NotFound();
            }

            return View(whatsAppIntegracao);
        }

        // POST: WhatsApp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var whatsAppIntegracao = await _context.WhatsAppIntegracoes.FindAsync(id);
            _context.WhatsAppIntegracoes.Remove(whatsAppIntegracao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: WhatsApp/EnviarMensagem
        public IActionResult EnviarMensagem()
        {
            return View();
        }

        // POST: WhatsApp/EnviarMensagem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarMensagem(string numeroDestino, string mensagem)
        {
            if (string.IsNullOrEmpty(numeroDestino) || string.IsNullOrEmpty(mensagem))
            {
                ModelState.AddModelError("", "Número de destino e mensagem são obrigatórios");
                return View();
            }

            var integracao = await _context.WhatsAppIntegracoes.FirstOrDefaultAsync(w => w.Ativo);
            if (integracao == null)
            {
                ModelState.AddModelError("", "Não há integração com WhatsApp configurada e ativa");
                return View();
            }

            // Usar o serviço para enviar a mensagem
            bool resultado = await _whatsAppService.EnviarMensagemTexto(numeroDestino, mensagem);
            
            if (resultado)
            {
                ViewBag.Sucesso = true;
                ViewBag.Mensagem = "Mensagem enviada com sucesso para " + numeroDestino;
            }
            else
            {
                ViewBag.Sucesso = false;
                ModelState.AddModelError("", "Erro ao enviar mensagem. Verifique as configurações e tente novamente.");
            }
            
            return View();
        }

        private bool WhatsAppIntegracaoExists(int id)
        {
            return _context.WhatsAppIntegracoes.Any(e => e.Id == id);
        }
    }
}