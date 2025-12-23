import { MenuCategory } from '../models/menu.model';

export const MENU_CONFIG: MenuCategory[] = [
  {
    id: 'dashboard',
    label: 'Dashboard',
    icon: 'bi-speedometer2',
    expanded: true,
    items: [
      {
        id: 'home',
        label: 'Visão Geral',
        icon: 'bi-house-door',
        route: '/dashboard'
      },
      {
        id: 'crm-dashboard',
        label: 'Dashboard CRM',
        icon: 'bi-people-fill',
        route: '/crm'
      },
      {
        id: 'erp-dashboard',
        label: 'Dashboard ERP',
        icon: 'bi-building',
        route: '/erp'
      }
    ]
  },
  {
    id: 'cadastros',
    label: 'Cadastros',
    icon: 'bi-folder-fill',
    expanded: false,
    items: [
      {
        id: 'clientes',
        label: 'Clientes',
        icon: 'bi-people',
        route: '/clientes'
      },
      {
        id: 'fornecedores',
        label: 'Fornecedores',
        icon: 'bi-truck',
        route: '/fornecedores'
      },
      {
        id: 'produtos',
        label: 'Produtos',
        icon: 'bi-box-seam',
        route: '/produtos'
      },
      {
        id: 'usuarios',
        label: 'Usuários',
        icon: 'bi-person-badge',
        route: '/configuracoes/usuarios'
      }
    ]
  },
  {
    id: 'vendas',
    label: 'Vendas',
    icon: 'bi-cart-check-fill',
    expanded: false,
    items: [
      {
        id: 'pdv',
        label: 'PDV (Ponto de Venda)',
        icon: 'bi-cash-register',
        route: '/pdv'
      },
      {
        id: 'pedidos-venda',
        label: 'Pedidos de Venda',
        icon: 'bi-cart-check',
        route: '/vendas/pedidos'
      },
      {
        id: 'nfce',
        label: 'NFC-e',
        icon: 'bi-receipt',
        route: '/nfce'
      },
      {
        id: 'orcamentos',
        label: 'Orçamentos',
        icon: 'bi-file-earmark-text',
        route: '/vendas/orcamentos'
      }
    ]
  },
  {
    id: 'compras',
    label: 'Compras',
    icon: 'bi-cart-plus-fill',
    expanded: false,
    items: [
      {
        id: 'pedidos-compra',
        label: 'Pedidos de Compra',
        icon: 'bi-file-earmark-plus',
        route: '/compras/pedidos'
      },
      {
        id: 'cotacoes',
        label: 'Cotações',
        icon: 'bi-calculator',
        route: '/compras/cotacoes'
      },
      {
        id: 'solicitacoes',
        label: 'Solicitações',
        icon: 'bi-clipboard-check',
        route: '/compras/solicitacoes'
      }
    ]
  },
  {
    id: 'estoque',
    label: 'Estoque',
    icon: 'bi-boxes',
    expanded: false,
    items: [
      {
        id: 'estoque-consulta',
        label: 'Consultar Estoque',
        icon: 'bi-search',
        route: '/estoque'
      },
      {
        id: 'movimentacoes',
        label: 'Movimentações',
        icon: 'bi-arrow-left-right',
        route: '/estoque/movimentacoes'
      },
      {
        id: 'inventario',
        label: 'Inventário',
        icon: 'bi-clipboard-data',
        route: '/estoque/inventario'
      },
      {
        id: 'transferencias',
        label: 'Transferências',
        icon: 'bi-arrow-repeat',
        route: '/estoque/transferencias'
      }
    ]
  },
  {
    id: 'financeiro',
    label: 'Financeiro',
    icon: 'bi-currency-dollar',
    expanded: false,
    items: [
      {
        id: 'contas-pagar',
        label: 'Contas a Pagar',
        icon: 'bi-arrow-down-circle',
        route: '/financeiro/contas-pagar'
      },
      {
        id: 'contas-receber',
        label: 'Contas a Receber',
        icon: 'bi-arrow-up-circle',
        route: '/financeiro/contas-receber'
      },
      {
        id: 'fluxo-caixa',
        label: 'Fluxo de Caixa',
        icon: 'bi-cash-stack',
        route: '/financeiro/fluxo-caixa'
      },
      {
        id: 'bancos',
        label: 'Contas Bancárias',
        icon: 'bi-bank',
        route: '/financeiro/bancos'
      },
      {
        id: 'conciliacao',
        label: 'Conciliação Bancária',
        icon: 'bi-check2-square',
        route: '/financeiro/conciliacao'
      }
    ]
  },
  {
    id: 'contabilidade',
    label: 'Contabilidade',
    icon: 'bi-calculator-fill',
    expanded: false,
    items: [
      {
        id: 'plano-contas',
        label: 'Plano de Contas',
        icon: 'bi-list-nested',
        route: '/erp/plano-contas'
      },
      {
        id: 'lancamentos',
        label: 'Lançamentos Contábeis',
        icon: 'bi-journal-text',
        route: '/erp/lancamentos'
      },
      {
        id: 'centro-custos',
        label: 'Centro de Custos',
        icon: 'bi-pie-chart',
        route: '/erp/centro-custos'
      },
      {
        id: 'balancete',
        label: 'Balancete',
        icon: 'bi-file-earmark-spreadsheet',
        route: '/erp/balancete'
      },
      {
        id: 'dre',
        label: 'DRE',
        icon: 'bi-graph-up-arrow',
        route: '/erp/dre'
      }
    ]
  },
  {
    id: 'producao',
    label: 'Produção (PCP)',
    icon: 'bi-gear-fill',
    expanded: false,
    items: [
      {
        id: 'ordens-producao',
        label: 'Ordens de Produção',
        icon: 'bi-clipboard-data',
        route: '/pcp/ordens'
      },
      {
        id: 'recursos',
        label: 'Recursos',
        icon: 'bi-tools',
        route: '/pcp/recursos'
      },
      {
        id: 'apontamentos',
        label: 'Apontamentos',
        icon: 'bi-clock-history',
        route: '/pcp/apontamentos'
      },
      {
        id: 'qualidade',
        label: 'Controle de Qualidade',
        icon: 'bi-shield-check',
        route: '/pcp/qualidade'
      }
    ]
  },
  {
    id: 'crm',
    label: 'CRM',
    icon: 'bi-people-fill',
    expanded: false,
    items: [
      {
        id: 'leads',
        label: 'Leads',
        icon: 'bi-person-plus',
        route: '/crm/leads'
      },
      {
        id: 'oportunidades',
        label: 'Oportunidades',
        icon: 'bi-bullseye',
        route: '/crm/oportunidades'
      },
      {
        id: 'campanhas',
        label: 'Campanhas',
        icon: 'bi-megaphone',
        route: '/crm/campanhas'
      },
      {
        id: 'propostas',
        label: 'Propostas Comerciais',
        icon: 'bi-file-earmark-check',
        route: '/crm/propostas'
      },
      {
        id: 'atividades',
        label: 'Atividades',
        icon: 'bi-calendar-event',
        route: '/crm/atividades'
      }
    ]
  },
  {
    id: 'projetos',
    label: 'Projetos',
    icon: 'bi-kanban',
    expanded: false,
    items: [
      {
        id: 'kanban',
        label: 'Kanban',
        icon: 'bi-kanban',
        route: '/kanban'
      },
      {
        id: 'tarefas',
        label: 'Tarefas',
        icon: 'bi-list-check',
        route: '/tarefas'
      },
      {
        id: 'scrum',
        label: 'Scrum',
        icon: 'bi-diagram-3',
        route: '/scrum'
      },
      {
        id: 'gantt',
        label: 'Gráfico de Gantt',
        icon: 'bi-bar-chart-steps',
        route: '/projetos/gantt'
      }
    ]
  },
  {
    id: 'relatorios',
    label: 'Relatórios',
    icon: 'bi-file-bar-graph-fill',
    expanded: false,
    items: [
      {
        id: 'rel-vendas',
        label: 'Relatórios de Vendas',
        icon: 'bi-graph-up',
        route: '/relatorios/vendas'
      },
      {
        id: 'rel-compras',
        label: 'Relatórios de Compras',
        icon: 'bi-cart3',
        route: '/relatorios/compras'
      },
      {
        id: 'rel-financeiro',
        label: 'Relatórios Financeiros',
        icon: 'bi-cash-coin',
        route: '/relatorios/financeiro'
      },
      {
        id: 'rel-estoque',
        label: 'Relatórios de Estoque',
        icon: 'bi-boxes',
        route: '/relatorios/estoque'
      },
      {
        id: 'rel-producao',
        label: 'Relatórios de Produção',
        icon: 'bi-gear',
        route: '/relatorios/producao'
      },
      {
        id: 'rel-contabil',
        label: 'Relatórios Contábeis',
        icon: 'bi-file-earmark-spreadsheet',
        route: '/relatorios/contabeis'
      },
      {
        id: 'rel-gerenciais',
        label: 'Relatórios Gerenciais',
        icon: 'bi-clipboard-data',
        route: '/relatorios/gerenciais'
      }
    ]
  },
  {
    id: 'ferramentas',
    label: 'Ferramentas',
    icon: 'bi-tools',
    expanded: false,
    items: [
      {
        id: 'no-code',
        label: 'No-Code Builder',
        icon: 'bi-bricks',
        route: '/no-code'
      },
      {
        id: 'sql-builder',
        label: 'SQL Builder',
        icon: 'bi-database',
        route: '/sql-builder'
      },
      {
        id: 'query-builder',
        label: 'Query Builder',
        icon: 'bi-code-square',
        route: '/query-builder'
      },
      {
        id: 'excel-chatbot',
        label: 'Excel Chatbot',
        icon: 'bi-robot',
        route: '/excel-chatbot'
      },
      {
        id: 'whatsapp',
        label: 'WhatsApp',
        icon: 'bi-whatsapp',
        route: '/whatsapp'
      }
    ]
  },
  {
    id: 'configuracoes',
    label: 'Configurações',
    icon: 'bi-gear-fill',
    expanded: false,
    items: [
      {
        id: 'empresa',
        label: 'Dados da Empresa',
        icon: 'bi-building',
        route: '/configuracoes/empresa'
      },
      {
        id: 'departamentos',
        label: 'Departamentos',
        icon: 'bi-diagram-2',
        route: '/configuracoes/departamentos'
      },
      {
        id: 'permissoes',
        label: 'Permissões',
        icon: 'bi-shield-lock',
        route: '/configuracoes/permissoes'
      },
      {
        id: 'temas',
        label: 'Temas',
        icon: 'bi-palette-fill',
        route: '/configuracoes/temas'
      },
      {
        id: 'menu',
        label: 'Personalizar Menu',
        icon: 'bi-list',
        route: '/configuracoes/menu'
      },
      {
        id: 'integracao',
        label: 'Integrações',
        icon: 'bi-plug',
        route: '/configuracoes/integracoes'
      },
      {
        id: 'backup',
        label: 'Backup',
        icon: 'bi-cloud-arrow-up',
        route: '/configuracoes/backup'
      },
      {
        id: 'privacidade',
        label: 'Privacidade',
        icon: 'bi-shield-check',
        route: '/privacidade'
      }
    ]
  }
];
