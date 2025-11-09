using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApp.Models;

namespace WebApp.Services
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MenuService> _logger;
        private readonly IMemoryCache _cache;
        private const string MENU_CACHE_KEY = "MenuItems_Active";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        public MenuService(ApplicationDbContext context, ILogger<MenuService> logger, IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync()
        {
            try
            {
                // Tentar buscar do cache primeiro
                if (_cache.TryGetValue(MENU_CACHE_KEY, out IEnumerable<MenuItem>? cachedMenu) && cachedMenu != null)
                {
                    _logger.LogDebug("Menu carregado do cache");
                    return cachedMenu;
                }

                _logger.LogInformation("Carregando menu do banco de dados");

                // Buscar do banco com query otimizada
                var menuItems = await _context.MenuItems
                    .AsNoTracking() // Melhor performance para leitura
                    .Include(m => m.SubMenus.Where(s => s.Ativo).OrderBy(s => s.Ordem))
                    .Where(m => m.Ativo && m.MenuPaiId == null)
                    .OrderBy(m => m.Ordem)
                    .ThenBy(m => m.Titulo)
                    .AsSplitQuery() // Otimiza queries com Include
                    .ToListAsync();

                // Armazenar no cache
                _cache.Set(MENU_CACHE_KEY, menuItems, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheDuration,
                    Priority = CacheItemPriority.High
                });

                _logger.LogInformation("Menu carregado e armazenado em cache. Total de itens: {Count}", menuItems.Count);
                return menuItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar itens do menu");
                return new List<MenuItem>();
            }
        }

        public void ClearMenuCache()
        {
            _cache.Remove(MENU_CACHE_KEY);
            _logger.LogInformation("Cache do menu limpo");
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
        {
            try
            {
                return await _context.MenuItems
                    .Include(m => m.MenuPai)
                    .Include(m => m.SubMenus.OrderBy(s => s.Ordem))
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar item do menu por ID: {Id}", id);
                return null;
            }
        }

        public async Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem)
        {
            try
            {
                menuItem.DataCriacao = DateTime.Now;
                _context.MenuItems.Add(menuItem);
                await _context.SaveChangesAsync();
                ClearMenuCache(); // Limpar cache após criar
                return menuItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar item do menu");
                throw;
            }
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem)
        {
            try
            {
                menuItem.DataAtualizacao = DateTime.Now;
                _context.MenuItems.Update(menuItem);
                await _context.SaveChangesAsync();
                ClearMenuCache(); // Limpar cache após atualizar
                return menuItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar item do menu");
                throw;
            }
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem != null)
                {
                    _context.MenuItems.Remove(menuItem);
                    await _context.SaveChangesAsync();
                    ClearMenuCache(); // Limpar cache após excluir
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir item do menu");
                return false;
            }
        }

        public async Task<bool> ToggleMenuItemStatusAsync(int id)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem != null)
                {
                    menuItem.Ativo = !menuItem.Ativo;
                    menuItem.DataAtualizacao = DateTime.Now;
                    await _context.SaveChangesAsync();
                    ClearMenuCache(); // Limpar cache após alterar status
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar status do item do menu");
                return false;
            }
        }

        public async Task<IEnumerable<MenuItem>> GetDuplicateMenuItemsAsync()
        {
            try
            {
                var allMenuItems = await _context.MenuItems.ToListAsync();
                var duplicates = new List<MenuItem>();

                // Agrupar por título, controller e action para encontrar duplicatas
                var groupedItems = allMenuItems
                    .GroupBy(m => new { m.Titulo, m.Controller, m.Action, m.MenuPaiId })
                    .Where(g => g.Count() > 1)
                    .ToList();

                foreach (var group in groupedItems)
                {
                    // Manter o primeiro item (mais antigo) e marcar os outros como duplicatas
                    var orderedItems = group.OrderBy(m => m.DataCriacao).ToList();
                    duplicates.AddRange(orderedItems.Skip(1)); // Pular o primeiro (mais antigo)
                }

                return duplicates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar itens duplicados do menu");
                return new List<MenuItem>();
            }
        }

        public async Task<int> RemoveDuplicateMenuItemsAsync()
        {
            try
            {
                var duplicates = await GetDuplicateMenuItemsAsync();
                var removedCount = 0;

                foreach (var duplicate in duplicates)
                {
                    // Verificar se o item tem submenus antes de remover
                    var hasSubMenus = await _context.MenuItems.AnyAsync(m => m.MenuPaiId == duplicate.Id);
                    
                    if (!hasSubMenus)
                    {
                        _context.MenuItems.Remove(duplicate);
                        removedCount++;
                        _logger.LogInformation("Item duplicado removido: {Titulo} (ID: {Id})", duplicate.Titulo, duplicate.Id);
                    }
                    else
                    {
                        _logger.LogWarning("Não foi possível remover item duplicado {Titulo} (ID: {Id}) pois possui submenus", duplicate.Titulo, duplicate.Id);
                    }
                }

                if (removedCount > 0)
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Removidos {Count} itens duplicados do menu", removedCount);
                }

                return removedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover itens duplicados do menu");
                return 0;
            }
        }
    }
}
