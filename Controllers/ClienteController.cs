using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(ApplicationDbContext context, ILogger<ClienteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return View(clientes);
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Endereco,CPF,EstadoCivil,Bairro,Email,Telefone,Cidade")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verifica se já existe um cliente com este CPF
                    var existingClienteByCPF = await _context.Clientes
                        .FirstOrDefaultAsync(c => c.CPF == cliente.CPF);

                    if (existingClienteByCPF != null)
                    {
                        ModelState.AddModelError("CPF", "Já existe um cliente cadastrado com este CPF.");
                        return View(cliente);
                    }

                    // Verifica se já existe um cliente com este email
                    var existingClienteByEmail = await _context.Clientes
                        .FirstOrDefaultAsync(c => c.Email == cliente.Email);

                    if (existingClienteByEmail != null)
                    {
                        ModelState.AddModelError("Email", "Já existe um cliente cadastrado com este email.");
                        return View(cliente);
                    }

                    cliente.DataCadastro = DateTime.Now;
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cliente cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar cliente");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao cadastrar o cliente. Tente novamente.");
                }
            }
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Endereco,CPF,EstadoCivil,Bairro,Email,Telefone,Cidade,DataCadastro")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verifica se já existe outro cliente com este CPF
                    var existingClienteByCPF = await _context.Clientes
                        .FirstOrDefaultAsync(c => c.CPF == cliente.CPF && c.Id != cliente.Id);

                    if (existingClienteByCPF != null)
                    {
                        ModelState.AddModelError("CPF", "Já existe outro cliente cadastrado com este CPF.");
                        return View(cliente);
                    }

                    // Verifica se já existe outro cliente com este email
                    var existingClienteByEmail = await _context.Clientes
                        .FirstOrDefaultAsync(c => c.Email == cliente.Email && c.Id != cliente.Id);

                    if (existingClienteByEmail != null)
                    {
                        ModelState.AddModelError("Email", "Já existe outro cliente cadastrado com este email.");
                        return View(cliente);
                    }

                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cliente atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
                    _logger.LogError(ex, "Erro ao atualizar cliente");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar o cliente. Tente novamente.");
                    return View(cliente);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente != null)
                {
                    _context.Clientes.Remove(cliente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cliente excluído com sucesso!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir cliente");
                TempData["ErrorMessage"] = "Ocorreu um erro ao excluir o cliente. Tente novamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
