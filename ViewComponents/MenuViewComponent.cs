using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly ILogger<MenuViewComponent> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthorizationPolicyProvider _policyProvider;

        public MenuViewComponent(
            IMenuService menuService,
            ILogger<MenuViewComponent> logger,
            IAuthorizationService authorizationService,
            IAuthorizationPolicyProvider policyProvider)
        {
            _menuService = menuService;
            _logger = logger;
            _authorizationService = authorizationService;
            _policyProvider = policyProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var menuItems = await _menuService.GetMenuItemsAsync();
                var filteredMenu = await FiltrarPorPermissaoAsync(menuItems);
                return View(filteredMenu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao renderizar menu dinâmico, carregando fallback estático");
                var fallback = GetStaticMenuItems();
                return View(fallback);
            }
        }

        private async Task<IEnumerable<MenuItem>> FiltrarPorPermissaoAsync(IEnumerable<MenuItem> menuItems)
        {
            var usuario = HttpContext.User;
            var filtered = new List<MenuItem>();

            foreach (var item in menuItems)
            {
                if (await PossuiPermissaoAsync(usuario, item))
                {
                    var clone = ClonarItem(item);
                    clone.SubMenus = (await FiltrarPorPermissaoAsync(item.SubMenus ?? Enumerable.Empty<MenuItem>())).ToList();
                    filtered.Add(clone);
                }
            }

            return filtered;
        }

        private async Task<bool> PossuiPermissaoAsync(ClaimsPrincipal usuario, MenuItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Controller) || string.IsNullOrWhiteSpace(item.Action))
            {
                return true;
            }

            var policyName = $"{item.Controller}:{item.Action}";
            var policy = await _policyProvider.GetPolicyAsync(policyName);

            if (policy == null)
            {
                return true;
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(usuario, null, policy);
            return authorizationResult.Succeeded;
        }

        private static MenuItem ClonarItem(MenuItem item)
        {
            return new MenuItem
            {
                Id = item.Id,
                Titulo = item.Titulo,
                Url = item.Url,
                Icone = item.Icone,
                Ordem = item.Ordem,
                Ativo = item.Ativo,
                AbrirNovaAba = item.AbrirNovaAba,
                Descricao = item.Descricao,
                Controller = item.Controller,
                Action = item.Action,
                Area = item.Area,
                MenuPaiId = item.MenuPaiId,
                EMenuPai = item.EMenuPai,
                SubMenus = new List<MenuItem>()
            };
        }

        private List<MenuItem> GetStaticMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Id = 1,
                    Titulo = "Dashboard",
                    Controller = "Home",
                    Action = "Index",
                    Icone = "bi-house",
                    Ordem = 1,
                    Ativo = true
                },
                new MenuItem
                {
                    Id = 2,
                    Titulo = "PCP",
                    Icone = "bi-diagram-3",
                    Ordem = 2,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 21,
                            Titulo = "Dashboard PCP",
                            Controller = "PCP",
                            Action = "Dashboard",
                            Icone = "bi-speedometer2",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 22,
                            Titulo = "Ordens de Produção",
                            Controller = "PCP",
                            Action = "OrdemProducao",
                            Icone = "bi-list-task",
                            Ordem = 2,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 23,
                            Titulo = "Recursos",
                            Controller = "PCP",
                            Action = "Recursos",
                            Icone = "bi-tools",
                            Ordem = 3,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 24,
                            Titulo = "Apontamentos",
                            Controller = "PCP",
                            Action = "Apontamento",
                            Icone = "bi-clock-history",
                            Ordem = 4,
                            Ativo = true
                        }
                    }
                },
                new MenuItem
                {
                    Id = 3,
                    Titulo = "PDV",
                    Icone = "bi-cash-register",
                    Ordem = 3,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 31,
                            Titulo = "Vendas",
                            Controller = "PDV",
                            Action = "Index",
                            Icone = "bi-cart",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 32,
                            Titulo = "NFC-e",
                            Controller = "PDV",
                            Action = "ImportarNFCe",
                            Icone = "bi-receipt",
                            Ordem = 2,
                            Ativo = true
                        }
                    }
                },
                new MenuItem
                {
                    Id = 4,
                    Titulo = "Produtos",
                    Controller = "Produto",
                    Action = "Index",
                    Icone = "bi-box",
                    Ordem = 4,
                    Ativo = true
                },
                new MenuItem
                {
                    Id = 5,
                    Titulo = "Clientes",
                    Controller = "Cliente",
                    Action = "Index",
                    Icone = "bi-people",
                    Ordem = 5,
                    Ativo = true
                },
                new MenuItem
                {
                    Id = 6,
                    Titulo = "Relatórios",
                    Icone = "bi-graph-up",
                    Ordem = 6,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 61,
                            Titulo = "Relatório de Vendas",
                            Controller = "Relatorio",
                            Action = "Vendas",
                            Icone = "bi-bar-chart",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 62,
                            Titulo = "Relatório PCP",
                            Controller = "RelatorioPCP",
                            Action = "Index",
                            Icone = "bi-pie-chart",
                            Ordem = 2,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 63,
                            Titulo = "SQL Builder",
                            Controller = "QueryBuilder",
                            Action = "Index",
                            Icone = "bi-database",
                            Ordem = 3,
                            Ativo = true
                        }
                    }
                },
                new MenuItem
                {
                    Id = 7,
                    Titulo = "Tarefas",
                    Icone = "bi-check2-square",
                    Ordem = 7,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 71,
                            Titulo = "Lista de Tarefas",
                            Controller = "Tarefa",
                            Action = "Index",
                            Icone = "bi-list-task",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 72,
                            Titulo = "Kanban",
                            Controller = "Tarefa",
                            Action = "Kanban",
                            Icone = "bi-kanban",
                            Ordem = 2,
                            Ativo = true
                        }
                    }
                },
                new MenuItem
                {
                    Id = 8,
                    Titulo = "Scrum",
                    Icone = "bi-diagram-2",
                    Ordem = 8,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 81,
                            Titulo = "Dashboard",
                            Controller = "Scrum",
                            Action = "Index",
                            Icone = "bi-speedometer2",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 82,
                            Titulo = "Product Backlog",
                            Controller = "Scrum",
                            Action = "Backlog",
                            Icone = "bi-list-ul",
                            Ordem = 2,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 83,
                            Titulo = "Sprint Planning",
                            Controller = "Scrum",
                            Action = "Planning",
                            Icone = "bi-calendar-event",
                            Ordem = 3,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 84,
                            Titulo = "Criar Sprint",
                            Controller = "Scrum",
                            Action = "CreateSprint",
                            Icone = "bi-plus-circle",
                            Ordem = 4,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 85,
                            Titulo = "Nova User Story",
                            Controller = "Scrum",
                            Action = "CreateUserStory",
                            Icone = "bi-card-text",
                            Ordem = 5,
                            Ativo = true
                        }
                    }
                },
                new MenuItem
                {
                    Id = 9,
                    Titulo = "Financeiro",
                    Icone = "bi-currency-dollar",
                    Ordem = 9,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 91,
                            Titulo = "Dashboard",
                            Controller = "Financeiro",
                            Action = "Index",
                            Icone = "bi-graph-up",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 92,
                            Titulo = "Contas a Pagar",
                            Controller = "Financeiro",
                            Action = "ContasPagar",
                            Icone = "bi-credit-card",
                            Ordem = 2,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 93,
                            Titulo = "Contas a Receber",
                            Controller = "Financeiro",
                            Action = "ContasReceber",
                            Icone = "bi-cash-coin",
                            Ordem = 3,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 94,
                            Titulo = "Fluxo de Caixa",
                            Controller = "Financeiro",
                            Action = "FluxoCaixa",
                            Icone = "bi-arrow-left-right",
                            Ordem = 4,
                            Ativo = true
                        }
                    }
                },
                new MenuItem
                {
                    Id = 10,
                    Titulo = "Estoque",
                    Icone = "bi-boxes",
                    Ordem = 10,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 101,
                            Titulo = "Dashboard",
                            Controller = "Estoque",
                            Action = "Index",
                            Icone = "bi-speedometer2",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 102,
                            Titulo = "Consulta",
                            Controller = "Estoque",
                            Action = "Consulta",
                            Icone = "bi-search",
                            Ordem = 2,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 103,
                            Titulo = "Entrada",
                            Controller = "Estoque",
                            Action = "Entrada",
                            Icone = "bi-plus-circle",
                            Ordem = 3,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 104,
                            Titulo = "Saída",
                            Controller = "Estoque",
                            Action = "Saida",
                            Icone = "bi-dash-circle",
                            Ordem = 4,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 105,
                            Titulo = "Movimentações",
                            Controller = "Estoque",
                            Action = "Movimentacoes",
                            Icone = "bi-arrow-repeat",
                            Ordem = 5,
                            Ativo = true
                        }
                    }
                },
                new MenuItem
                {
                    Id = 11,
                    Titulo = "Compras",
                    Icone = "bi-cart-plus",
                    Ordem = 11,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 111,
                            Titulo = "Fornecedores",
                            Controller = "Fornecedor",
                            Action = "Index",
                            Icone = "bi-building",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 112,
                            Titulo = "Pedidos",
                            Controller = "Compras",
                            Action = "Index",
                            Icone = "bi-clipboard-check",
                            Ordem = 2,
                            Ativo = true
                        }
                    }
                },
                new MenuItem
                {
                    Id = 12,
                    Titulo = "Configurações",
                    Icone = "bi-gear",
                    Ordem = 12,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 121,
                            Titulo = "Usuários",
                            Controller = "User",
                            Action = "Index",
                            Icone = "bi-person-gear",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 122,
                            Titulo = "Permissões",
                            Controller = "Permissao",
                            Action = "Index",
                            Icone = "bi-shield-check",
                            Ordem = 2,
                            Ativo = true
                        }
                    }
                }
            };
        }
    }
}
