using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class FornecedorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FornecedorController> _logger;

        public FornecedorController(ApplicationDbContext context, ILogger<FornecedorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var fornecedores = await _context.Fornecedores
                    .Where(f => f.Ativo)
                    .OrderBy(f => f.RazaoSocial)
                    .ToListAsync();

                return View(fornecedores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar fornecedores");
                TempData["Error"] = "Erro ao carregar a lista de fornecedores.";
                return View(new List<Fornecedor>());
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var fornecedor = await _context.Fornecedores
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (fornecedor == null)
                {
                    return NotFound();
                }

                return View(fornecedor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar fornecedor {Id}", id);
                TempData["Error"] = "Erro ao carregar os detalhes do fornecedor.";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RazaoSocial,NomeFantasia,CnpjCpf,InscricaoEstadual,InscricaoMunicipal,Endereco,Numero,Complemento,Bairro,Cidade,Estado,Cep,Telefone,Celular,Email,Site,Contato,Observacoes,Ativo")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    fornecedor.DataCadastro = DateTime.Now;
                    _context.Add(fornecedor);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Fornecedor cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar fornecedor");
                    TempData["Error"] = "Erro ao cadastrar fornecedor.";
                }
            }
            return View(fornecedor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var fornecedor = await _context.Fornecedores.FindAsync(id);
                if (fornecedor == null)
                {
                    return NotFound();
                }
                return View(fornecedor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar fornecedor para edição {Id}", id);
                TempData["Error"] = "Erro ao carregar fornecedor para edição.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RazaoSocial,NomeFantasia,CnpjCpf,InscricaoEstadual,InscricaoMunicipal,Endereco,Numero,Complemento,Bairro,Cidade,Estado,Cep,Telefone,Celular,Email,Site,Contato,Observacoes,Ativo,DataCadastro")] Fornecedor fornecedor)
        {
            if (id != fornecedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    fornecedor.DataAtualizacao = DateTime.Now;
                    _context.Update(fornecedor);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Fornecedor atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!FornecedorExists(fornecedor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Erro de concorrência ao atualizar fornecedor {Id}", id);
                        TempData["Error"] = "Erro de concorrência. Tente novamente.";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar fornecedor {Id}", id);
                    TempData["Error"] = "Erro ao atualizar fornecedor.";
                }
            }
            return View(fornecedor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var fornecedor = await _context.Fornecedores
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (fornecedor == null)
                {
                    return NotFound();
                }

                return View(fornecedor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar fornecedor para exclusão {Id}", id);
                TempData["Error"] = "Erro ao carregar fornecedor.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var fornecedor = await _context.Fornecedores.FindAsync(id);
                if (fornecedor != null)
                {
                    // Soft delete - apenas desativa o fornecedor
                    fornecedor.Ativo = false;
                    fornecedor.DataAtualizacao = DateTime.Now;
                    _context.Update(fornecedor);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Fornecedor desativado com sucesso!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desativar fornecedor {Id}", id);
                TempData["Error"] = "Erro ao desativar fornecedor.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FornecedorExists(int id)
        {
            return _context.Fornecedores.Any(e => e.Id == id);
        }
    }
}
