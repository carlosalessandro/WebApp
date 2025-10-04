using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApp.Services;
using WebApp.Models;


namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMenuService _menuService;

        public HomeController(ILogger<HomeController> logger, IMenuService menuService)
        {
            _logger = logger;
            _menuService = menuService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Inicializa o item de gerenciamento de menu se não existir
            await InitializeMenuManagementItem();
            
            // Passa informações do usuário para a view
            ViewBag.IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
            ViewBag.UserName = User.Identity?.Name;
            ViewBag.UserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            return View();
        }

        private async Task InitializeMenuManagementItem()
        {
            try
            {
                // Fazer uma única consulta ao banco para obter todos os itens
                var allMenuItems = await _menuService.GetMenuItemsAsync();
                
                // Inicializar todos os menus de uma vez
                await InitializeAllMenuItems(allMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inicializar itens do menu");
            }
        }

        private async Task InitializeAllMenuItems(IEnumerable<MenuItem> existingItems)
        {
            var itemsToCreate = new List<MenuItem>();
            
            // 1. Gerenciar Menu
            if (!existingItems.Any(m => m.Controller == "Menu" && m.Action == "Index"))
            {
                itemsToCreate.Add(new MenuItem
                {
                    Titulo = "Gerenciar Menu",
                    Controller = "Menu",
                    Action = "Index",
                    Icone = "bi-gear",
                    Ordem = 5,
                    Ativo = true,
                    AbrirNovaAba = false,
                    DataCriacao = DateTime.Now
                });
            }

            // 2. Remover itens de login/account
            await RemoveLoginMenuItems(existingItems);

            // 3. Menu de Permissões
            AddPermissionMenuItems(existingItems, itemsToCreate);

            // 4. Menu de Configuração
            AddConfigurationMenuItems(existingItems, itemsToCreate);

            // 5. Menu de Relatórios
            AddReportsMenuItems(existingItems, itemsToCreate);

            // 6. Menu Kanban
            AddKanbanMenuItems(existingItems, itemsToCreate);

            // 7. Menu Dashboard
            AddDashboardMenuItems(existingItems, itemsToCreate);

            // Criar todos os itens de uma vez
            foreach (var item in itemsToCreate)
            {
                await _menuService.CreateMenuItemAsync(item);
            }

            if (itemsToCreate.Any())
            {
                _logger.LogInformation("Inicializados {Count} itens do menu", itemsToCreate.Count);
            }
        }

        private async Task RemoveLoginMenuItems(IEnumerable<MenuItem> existingItems)
        {
            try
            {
                var loginItems = existingItems.Where(m => 
                    m.Controller == "Account" || 
                    m.Titulo.ToLower().Contains("login") ||
                    m.Titulo.ToLower().Contains("entrar") ||
                    m.Titulo.ToLower().Contains("sair") ||
                    m.Titulo.ToLower().Contains("logout"));

                foreach (var item in loginItems)
                {
                    await _menuService.DeleteMenuItemAsync(item.Id);
                    _logger.LogInformation("Item de menu de login removido: {Titulo}", item.Titulo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover itens de menu de login");
            }
        }

        private void AddPermissionMenuItems(IEnumerable<MenuItem> existingItems, List<MenuItem> itemsToCreate)
        {
            try
            {
                // Adicionar item "Permissões" como menu pai
                if (!existingItems.Any(m => m.Titulo == "Permissões"))
                {
                    itemsToCreate.Add(new MenuItem
                    {
                        Titulo = "Permissões",
                        Icone = "bi-shield-check",
                        Ordem = 6,
                        Ativo = true,
                        EMenuPai = true,
                        DataCriacao = DateTime.Now
                    });
                }

                // Adicionar submenu "Gerenciar Permissões"
                if (!existingItems.Any(m => m.Titulo == "Gerenciar Permissões"))
                {
                    itemsToCreate.Add(new MenuItem
                    {
                        Titulo = "Gerenciar Permissões",
                        Controller = "Permissao",
                        Action = "Index",
                        Icone = "bi-gear",
                        Ordem = 1,
                        Ativo = true,
                        DataCriacao = DateTime.Now
                    });
                }

                // Adicionar submenu "Usuários"
                if (!existingItems.Any(m => m.Titulo == "Usuários"))
                {
                    itemsToCreate.Add(new MenuItem
                    {
                        Titulo = "Usuários",
                        Controller = "User",
                        Action = "Index",
                        Icone = "bi-people",
                        Ordem = 2,
                        Ativo = true,
                        DataCriacao = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar itens de menu de permissões");
            }
        }

        private void AddConfigurationMenuItems(IEnumerable<MenuItem> existingItems, List<MenuItem> itemsToCreate)
        {
            try
            {
                // Verificar se já existe menu "Configuração"
                var configMenu = existingItems.FirstOrDefault(m => m.Titulo == "Configuração");
                
                if (configMenu == null)
                {
                    // Criar menu "Configuração" como menu pai
                    configMenu = new MenuItem
                    {
                        Titulo = "Configuração",
                        Icone = "bi-gear-fill",
                        Ordem = 7,
                        Ativo = true,
                        EMenuPai = true,
                        DataCriacao = DateTime.Now
                    };
                    
                    itemsToCreate.Add(configMenu);
                }

                // Adicionar submenus se o menu pai existe ou será criado
                if (configMenu != null)
                {
                    // Adicionar submenu "Permissão"
                    if (!existingItems.Any(m => m.Titulo == "Permissão" && m.MenuPaiId == configMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Permissão",
                            Controller = "Permissao",
                            Action = "Index",
                            Icone = "bi-shield-check",
                            Ordem = 1,
                            Ativo = true,
                            MenuPaiId = configMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }

                    // Adicionar submenu "Gerencia Menu"
                    if (!existingItems.Any(m => m.Titulo == "Gerencia Menu" && m.MenuPaiId == configMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Gerencia Menu",
                            Controller = "Menu",
                            Action = "Index",
                            Icone = "bi-list-ul",
                            Ordem = 2,
                            Ativo = true,
                            MenuPaiId = configMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }

                    // Adicionar submenu "Gerencia Permissão"
                    if (!existingItems.Any(m => m.Titulo == "Gerencia Permissão" && m.MenuPaiId == configMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Gerencia Permissão",
                            Controller = "UsuarioPermissao",
                            Action = "Manage",
                            Icone = "bi-person-gear",
                            Ordem = 3,
                            Ativo = true,
                            MenuPaiId = configMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar itens de menu de configuração");
            }
        }

        private void AddReportsMenuItems(IEnumerable<MenuItem> existingItems, List<MenuItem> itemsToCreate)
        {
            try
            {
                // Verificar se já existe menu "Relatórios"
                var reportsMenu = existingItems.FirstOrDefault(m => m.Titulo == "Relatórios");
                
                if (reportsMenu == null)
                {
                    // Criar menu "Relatórios" como menu pai
                    reportsMenu = new MenuItem
                    {
                        Titulo = "Relatórios",
                        Icone = "bi-graph-up",
                        Ordem = 8,
                        Ativo = true,
                        EMenuPai = true,
                        DataCriacao = DateTime.Now
                    };
                    
                    itemsToCreate.Add(reportsMenu);
                }

                if (reportsMenu != null)
                {
                    // Adicionar submenu "Relatórios Gerais"
                    if (!existingItems.Any(m => m.Titulo == "Relatórios Gerais" && m.MenuPaiId == reportsMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Relatórios Gerais",
                            Controller = "Relatorio",
                            Action = "Index",
                            Icone = "bi-graph-up",
                            Ordem = 1,
                            Ativo = true,
                            MenuPaiId = reportsMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }

                    // Adicionar submenu "Relatório de Clientes"
                    if (!existingItems.Any(m => m.Titulo == "Relatório de Clientes" && m.MenuPaiId == reportsMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Relatório de Clientes",
                            Controller = "Relatorio",
                            Action = "Clientes",
                            Icone = "bi-people",
                            Ordem = 2,
                            Ativo = true,
                            MenuPaiId = reportsMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar itens de menu de relatórios");
            }
        }

        private void AddKanbanMenuItems(IEnumerable<MenuItem> existingItems, List<MenuItem> itemsToCreate)
        {
            try
            {
                // Verificar se já existe menu "Kanban"
                var kanbanMenu = existingItems.FirstOrDefault(m => m.Titulo == "Kanban");
                
                if (kanbanMenu == null)
                {
                    // Criar menu "Kanban" como menu pai
                    kanbanMenu = new MenuItem
                    {
                        Titulo = "Kanban",
                        Icone = "bi-kanban",
                        Ordem = 9,
                        Ativo = true,
                        EMenuPai = true,
                        DataCriacao = DateTime.Now
                    };
                    
                    itemsToCreate.Add(kanbanMenu);
                }

                if (kanbanMenu != null)
                {
                    // Adicionar submenu "Kanban Board"
                    if (!existingItems.Any(m => m.Titulo == "Kanban Board" && m.MenuPaiId == kanbanMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Kanban Board",
                            Controller = "Tarefa",
                            Action = "Kanban",
                            Icone = "bi-kanban",
                            Ordem = 1,
                            Ativo = true,
                            MenuPaiId = kanbanMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }

                    // Adicionar submenu "Lista de Tarefas"
                    if (!existingItems.Any(m => m.Titulo == "Lista de Tarefas" && m.MenuPaiId == kanbanMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Lista de Tarefas",
                            Controller = "Tarefa",
                            Action = "Index",
                            Icone = "bi-list-task",
                            Ordem = 2,
                            Ativo = true,
                            MenuPaiId = kanbanMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar itens de menu Kanban");
            }
        }

        private void AddDashboardMenuItems(IEnumerable<MenuItem> existingItems, List<MenuItem> itemsToCreate)
        {
            try
            {
                // Verificar se já existe menu "Dashboard"
                var dashboardMenu = existingItems.FirstOrDefault(m => m.Titulo == "Dashboard");
                
                if (dashboardMenu == null)
                {
                    // Criar menu "Dashboard" como menu pai
                    dashboardMenu = new MenuItem
                    {
                        Titulo = "Dashboard",
                        Icone = "bi-speedometer2",
                        Ordem = 10,
                        Ativo = true,
                        EMenuPai = true,
                        DataCriacao = DateTime.Now
                    };
                    
                    itemsToCreate.Add(dashboardMenu);
                }

                if (dashboardMenu != null)
                {
                    // Adicionar submenu "Dashboard Geral"
                    if (!existingItems.Any(m => m.Titulo == "Dashboard Geral" && m.MenuPaiId == dashboardMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Dashboard Geral",
                            Controller = "Dashboard",
                            Action = "Index",
                            Icone = "bi-speedometer2",
                            Ordem = 1,
                            Ativo = true,
                            MenuPaiId = dashboardMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }

                    // Adicionar submenu "Dashboard Clientes"
                    if (!existingItems.Any(m => m.Titulo == "Dashboard Clientes" && m.MenuPaiId == dashboardMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Dashboard Clientes",
                            Controller = "Dashboard",
                            Action = "Clientes",
                            Icone = "bi-people",
                            Ordem = 2,
                            Ativo = true,
                            MenuPaiId = dashboardMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }

                    // Adicionar submenu "Dashboard Tarefas"
                    if (!existingItems.Any(m => m.Titulo == "Dashboard Tarefas" && m.MenuPaiId == dashboardMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Dashboard Tarefas",
                            Controller = "Dashboard",
                            Action = "Tarefas",
                            Icone = "bi-kanban",
                            Ordem = 3,
                            Ativo = true,
                            MenuPaiId = dashboardMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }

                    // Adicionar submenu "Dashboard Usuários"
                    if (!existingItems.Any(m => m.Titulo == "Dashboard Usuários" && m.MenuPaiId == dashboardMenu.Id))
                    {
                        itemsToCreate.Add(new MenuItem
                        {
                            Titulo = "Dashboard Usuários",
                            Controller = "Dashboard",
                            Action = "Usuarios",
                            Icone = "bi-people-fill",
                            Ordem = 4,
                            Ativo = true,
                            MenuPaiId = dashboardMenu.Id,
                            DataCriacao = DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar itens de menu Dashboard");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    internal class ErrorViewModel
    {
        public string? RequestId { get; set; }
    }
}
