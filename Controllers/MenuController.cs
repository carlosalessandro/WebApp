using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly ILogger<MenuController> _logger;
        private readonly ApplicationDbContext _context;

        public MenuController(IMenuService menuService, ILogger<MenuController> logger, ApplicationDbContext context)
        {
            _menuService = menuService;
            _logger = logger;
            _context = context;
        }

        // GET: Menu
        public async Task<IActionResult> Index()
        {
            var menuItems = await _menuService.GetMenuItemsAsync();
            return View(menuItems);
        }

        // GET: Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _menuService.GetMenuItemByIdAsync(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: Menu/Create
        public async Task<IActionResult> Create()
        {
            await PopulateMenuPaiDropdown();
            return View();
        }

        // POST: Menu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Url,Icone,Ordem,Ativo,AbrirNovaAba,Descricao,Controller,Action,Area,MenuPaiId,EMenuPai")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _menuService.CreateMenuItemAsync(menuItem);
                    TempData["SuccessMessage"] = "Item do menu criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar item do menu");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar o item do menu. Tente novamente.");
                }
            }
            await PopulateMenuPaiDropdown();
            return View(menuItem);
        }

        // GET: Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _menuService.GetMenuItemByIdAsync(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }

            await PopulateMenuPaiDropdown(menuItem.Id);
            return View(menuItem);
        }

        // POST: Menu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Url,Icone,Ordem,Ativo,AbrirNovaAba,Descricao,Controller,Action,Area,MenuPaiId,EMenuPai,DataCriacao")] MenuItem menuItem)
        {
            if (id != menuItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _menuService.UpdateMenuItemAsync(menuItem);
                    TempData["SuccessMessage"] = "Item do menu atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar item do menu");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar o item do menu. Tente novamente.");
                }
            }
            await PopulateMenuPaiDropdown(menuItem.Id);
            return View(menuItem);
        }

        // GET: Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _menuService.GetMenuItemByIdAsync(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _menuService.DeleteMenuItemAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Item do menu excluído com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Item do menu não encontrado.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir item do menu");
                TempData["ErrorMessage"] = "Ocorreu um erro ao excluir o item do menu. Tente novamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Menu/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var result = await _menuService.ToggleMenuItemStatusAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Status do item do menu alterado com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Item do menu não encontrado.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar status do item do menu");
                TempData["ErrorMessage"] = "Ocorreu um erro ao alterar o status do item do menu. Tente novamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Menu/Duplicates
        public async Task<IActionResult> Duplicates()
        {
            var duplicates = await _menuService.GetDuplicateMenuItemsAsync();
            return View(duplicates);
        }

        // POST: Menu/RemoveDuplicates
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDuplicates()
        {
            try
            {
                var removedCount = await _menuService.RemoveDuplicateMenuItemsAsync();
                if (removedCount > 0)
                {
                    TempData["SuccessMessage"] = $"Removidos {removedCount} itens duplicados do menu com sucesso!";
                }
                else
                {
                    TempData["InfoMessage"] = "Nenhum item duplicado encontrado.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover itens duplicados do menu");
                TempData["ErrorMessage"] = "Ocorreu um erro ao remover os itens duplicados. Tente novamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Menu/ReorganizeByModules
        public IActionResult ReorganizeByModules()
        {
            return View();
        }

        // POST: Menu/ReorganizeByModules
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReorganizeByModulesConfirm()
        {
            try
            {
                var sqlScript = await System.IO.File.ReadAllTextAsync("SQL/ReorganizarMenusPorModulo.sql");
                
                // Dividir por comandos e executar cada um
                var commands = sqlScript.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var command in commands)
                {
                    var trimmedCommand = command.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedCommand) && !trimmedCommand.StartsWith("--"))
                    {
                        await _context.Database.ExecuteSqlRawAsync(trimmedCommand);
                    }
                }

                // Limpar cache do menu após reorganização
                _menuService.ClearMenuCache();

                TempData["SuccessMessage"] = "Menus reorganizados por módulos com sucesso! Cache limpo.";
                _logger.LogInformation("Menus reorganizados por módulos e cache limpo");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reorganizar menus por módulos");
                TempData["ErrorMessage"] = $"Erro ao reorganizar menus: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Menu/ClearCache
        public IActionResult ClearCache()
        {
            _menuService.ClearMenuCache();
            TempData["SuccessMessage"] = "Cache do menu limpo com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateMenuPaiDropdown(int? excludeId = null)
        {
            var menuItems = await _menuService.GetMenuItemsAsync();
            var menuPaiItems = menuItems.Where(m => m.EMenuPai && (excludeId == null || m.Id != excludeId))
                                      .OrderBy(m => m.Titulo)
                                      .Select(m => new { m.Id, m.Titulo })
                                      .ToList();

            ViewBag.MenuPaiId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(menuPaiItems, "Id", "Titulo");
        }
    }
}

