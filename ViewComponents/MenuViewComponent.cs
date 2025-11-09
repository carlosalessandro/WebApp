using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            var menuItems = GetStaticMenuItems();
            return Task.FromResult<IViewComponentResult>(View(menuItems));
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
                    Controller = "Tarefa",
                    Action = "Index",
                    Icone = "bi-check2-square",
                    Ordem = 7,
                    Ativo = true
                },
                new MenuItem
                {
                    Id = 8,
                    Titulo = "Financeiro",
                    Icone = "bi-currency-dollar",
                    Ordem = 8,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 81,
                            Titulo = "Dashboard",
                            Controller = "Financeiro",
                            Action = "Index",
                            Icone = "bi-graph-up",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 82,
                            Titulo = "Contas a Pagar",
                            Controller = "Financeiro",
                            Action = "ContasPagar",
                            Icone = "bi-credit-card",
                            Ordem = 2,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 83,
                            Titulo = "Contas a Receber",
                            Controller = "Financeiro",
                            Action = "ContasReceber",
                            Icone = "bi-cash-coin",
                            Ordem = 3,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 84,
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
                    Id = 9,
                    Titulo = "Estoque",
                    Icone = "bi-boxes",
                    Ordem = 9,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 91,
                            Titulo = "Dashboard",
                            Controller = "Estoque",
                            Action = "Index",
                            Icone = "bi-speedometer2",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 92,
                            Titulo = "Consulta",
                            Controller = "Estoque",
                            Action = "Consulta",
                            Icone = "bi-search",
                            Ordem = 2,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 93,
                            Titulo = "Entrada",
                            Controller = "Estoque",
                            Action = "Entrada",
                            Icone = "bi-plus-circle",
                            Ordem = 3,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 94,
                            Titulo = "Saída",
                            Controller = "Estoque",
                            Action = "Saida",
                            Icone = "bi-dash-circle",
                            Ordem = 4,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 95,
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
                    Id = 10,
                    Titulo = "Compras",
                    Icone = "bi-cart-plus",
                    Ordem = 10,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 101,
                            Titulo = "Fornecedores",
                            Controller = "Fornecedor",
                            Action = "Index",
                            Icone = "bi-building",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 102,
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
                    Id = 11,
                    Titulo = "Configurações",
                    Icone = "bi-gear",
                    Ordem = 11,
                    Ativo = true,
                    EMenuPai = true,
                    SubMenus = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Id = 111,
                            Titulo = "Usuários",
                            Controller = "User",
                            Action = "Index",
                            Icone = "bi-person-gear",
                            Ordem = 1,
                            Ativo = true
                        },
                        new MenuItem
                        {
                            Id = 112,
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
