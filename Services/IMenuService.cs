using WebApp.Models;

namespace WebApp.Services
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuItem>> GetMenuItemsAsync();
        Task<MenuItem?> GetMenuItemByIdAsync(int id);
        Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem);
        Task<bool> DeleteMenuItemAsync(int id);
        Task<bool> ToggleMenuItemStatusAsync(int id);
        Task<IEnumerable<MenuItem>> GetDuplicateMenuItemsAsync();
        Task<int> RemoveDuplicateMenuItemsAsync();
    }
}
