# 📁 Estrutura do Projeto - ERP System

## 🎯 Organização por Módulos

O projeto foi reestruturado seguindo uma arquitetura modular, separando funcionalidades por domínio de negócio.

## 📂 Estrutura de Diretórios

### Backend (C# / ASP.NET Core)

```
WebApp/
├── Controllers/
│   ├── Account/
│   │   └── AccountController.cs
│   ├── Cadastros/
│   │   ├── ClienteController.cs
│   │   ├── FornecedorController.cs
│   │   └── ProdutoController.cs
│   ├── Vendas/
│   │   ├── PDVController.cs
│   │   └── VendasController.cs (a criar)
│   ├── Estoque/
│   │   └── EstoqueController.cs
│   ├── Financeiro/
│   │   └── FinanceiroController.cs
│   ├── Compras/
│   │   └── ComprasController.cs
│   ├── Producao/
│   │   ├── PCPController.cs
│   │   └── RelatorioPCPController.cs
│   ├── CRM/
│   │   └── CRMController.cs
│   ├── ERP/
│   │   └── ERPController.cs
│   ├── Projetos/
│   │   ├── TarefaController.cs
│   │   └── ScrumController.cs
│   ├── Ferramentas/
│   │   ├── NoCodeController.cs
│   │   ├── SqlBuilderController.cs
│   │   ├── QueryBuilderController.cs
│   │   ├── ExcelChatbotController.cs
│   │   └── WhatsAppController.cs
│   ├── Relatorios/
│   │   └── RelatorioController.cs
│   ├── Configuracoes/
│   │   ├── UserController.cs
│   │   ├── PermissaoController.cs
│   │   ├── UsuarioPermissaoController.cs
│   │   ├── MenuController.cs
│   │   └── ThemeController.cs
│   ├── Dashboard/
│   │   └── DashboardController.cs
│   └── Shared/
│       ├── HomeController.cs
│       ├── DiagramController.cs
│       ├── SqlJoinDemoController.cs
│       └── TestController.cs
│
├── Models/
│   ├── Account/
│   │   └── User.cs
│   ├── Cadastros/
│   │   ├── Cliente.cs
│   │   ├── Fornecedor.cs
│   │   └── Produto.cs
│   ├── Vendas/
│   │   ├── Venda.cs
│   │   ├── ItemVenda.cs
│   │   └── NFCe.cs
│   ├── Estoque/
│   │   └── Estoque.cs
│   ├── Financeiro/
│   │   ├── ContaPagar.cs
│   │   ├── ContaReceber.cs
│   │   ├── ContaBancaria.cs
│   │   ├── CategoriaFinanceira.cs
│   │   └── MovimentacaoFinanceira.cs
│   ├── Compras/
│   │   ├── PedidoCompra.cs
│   │   └── ItemPedidoCompra.cs (a criar)
│   ├── Producao/
│   │   ├── OrdemProducao.cs
│   │   ├── Recurso.cs
│   │   ├── RecursoAlocado.cs
│   │   ├── ApontamentoProducao.cs
│   │   └── RelatorioPCP.cs
│   ├── CRM/
│   │   ├── Lead.cs
│   │   ├── Oportunidade.cs
│   │   ├── CampanhaMarketing.cs
│   │   ├── PropostaComercial.cs
│   │   └── AtividadeCRM.cs
│   ├── ERP/
│   │   ├── PlanoContas.cs
│   │   ├── LancamentoContabil.cs
│   │   ├── CentroCusto.cs
│   │   └── Departamento.cs
│   ├── Projetos/
│   │   ├── Tarefa.cs
│   │   ├── Sprint.cs
│   │   ├── UserStory.cs
│   │   └── TaskUserStory.cs
│   ├── Ferramentas/
│   │   ├── DiagramModel.cs
│   │   ├── SqlQueryModel.cs
│   │   ├── ExcelChatbot.cs
│   │   └── WhatsAppIntegracao.cs
│   ├── Configuracoes/
│   │   ├── MenuItem.cs
│   │   ├── Categoria.cs
│   │   ├── Permissao.cs
│   │   ├── UsuarioPermissao.cs
│   │   └── ThemeConfig.cs
│   └── ApplicationDbContext.cs
│
├── Services/
│   ├── Menu/
│   │   └── MenuService.cs
│   ├── Vendas/
│   │   └── NFCeService.cs
│   └── Comunicacao/
│       └── WhatsAppService.cs
│
├── Views/
│   ├── Account/
│   ├── Cadastros/
│   ├── Vendas/
│   ├── Financeiro/
│   ├── Relatorios/
│   └── Shared/
│
└── Scripts/
    └── SeedDefaultThemes.cs
```

### Frontend (Angular)

```
ClientApp/src/app/
├── components/
│   ├── sidebar/
│   │   ├── sidebar.component.ts
│   │   ├── sidebar.component.html
│   │   └── sidebar.component.css
│   ├── dashboard/
│   │   └── (a criar)
│   ├── projetos/
│   │   ├── kanban/
│   │   └── tarefa-list/
│   ├── vendas/
│   │   └── (a criar)
│   ├── cadastros/
│   │   └── (a criar)
│   ├── estoque/
│   │   └── (a criar)
│   ├── financeiro/
│   │   └── (a criar)
│   ├── compras/
│   │   └── (a criar)
│   ├── producao/
│   │   └── (a criar)
│   ├── crm/
│   │   └── (a criar)
│   ├── erp/
│   │   └── (a criar)
│   ├── ferramentas/
│   │   ├── no-code-builder/
│   │   ├── sql-builder/
│   │   ├── sql-query-builder/
│   │   ├── sql-join-builder/
│   │   ├── component-palette/
│   │   └── diagram-canvas/
│   ├── relatorios/
│   │   └── (a criar)
│   └── configuracoes/
│       └── theme-config/
│
├── services/
│   ├── theme.service.ts
│   └── (outros serviços a criar)
│
├── models/
│   └── menu.model.ts
│
├── config/
│   └── menu.config.ts
│
└── app.component.ts
```

## 🗂️ Módulos do Sistema

### 1. 📊 Dashboard
- Visão geral do sistema
- Indicadores e métricas
- Gráficos e análises

### 2. 📋 Gestão de Projetos
- **Kanban**: Quadro kanban para gestão visual
- **Tarefas**: Lista de tarefas
- **Scrum**: Gestão ágil com sprints

### 3. 🛒 Vendas & PDV
- **PDV**: Ponto de venda
- **Vendas**: Gestão de vendas
- **NFC-e**: Emissão de notas fiscais

### 4. 📁 Cadastros
- **Clientes**: Cadastro de clientes
- **Fornecedores**: Cadastro de fornecedores
- **Produtos**: Cadastro de produtos

### 5. 📦 Estoque
- **Consulta**: Visualização de estoque
- **Movimentações**: Entradas e saídas
- **Inventário**: Contagem de estoque

### 6. 💰 Financeiro
- **Contas a Pagar**: Gestão de pagamentos
- **Contas a Receber**: Gestão de recebimentos
- **Fluxo de Caixa**: Controle de caixa
- **Contas Bancárias**: Gestão de contas

### 7. 🛍️ Compras
- **Pedidos de Compra**: Gestão de pedidos
- **Cotações**: Cotações de fornecedores

### 8. ⚙️ Produção (PCP)
- **Ordens de Produção**: Gestão de ordens
- **Recursos**: Gestão de recursos
- **Apontamentos**: Registro de produção
- **Relatórios**: Relatórios de produção

### 9. 👥 CRM
- **Leads**: Gestão de leads
- **Oportunidades**: Gestão de oportunidades
- **Campanhas**: Campanhas de marketing
- **Propostas**: Propostas comerciais

### 10. 🏢 ERP Avançado
- **Contabilidade**: Gestão contábil
- **Plano de Contas**: Estrutura contábil
- **Centro de Custos**: Gestão de custos
- **Departamentos**: Gestão de departamentos

### 11. 🔧 Ferramentas
- **No-Code Builder**: Construtor visual
- **SQL Builder**: Construtor de SQL
- **Query Builder**: Construtor de queries
- **Excel Chatbot**: Chatbot para Excel
- **WhatsApp**: Integração WhatsApp

### 12. 📈 Relatórios
- **Vendas**: Relatórios de vendas
- **Financeiro**: Relatórios financeiros
- **Estoque**: Relatórios de estoque

### 13. ⚙️ Configurações
- **Usuários**: Gestão de usuários
- **Permissões**: Gestão de permissões
- **Temas**: Personalização visual
- **Menu**: Configuração de menu
- **Sistema**: Configurações gerais

## 🎨 Menu Lateral Organizado

O menu lateral foi completamente reestruturado com:

- ✅ **Categorias expansíveis**: Agrupa itens relacionados
- ✅ **Busca integrada**: Filtra itens do menu
- ✅ **Ícones intuitivos**: Facilita identificação visual
- ✅ **Badges**: Notificações e contadores
- ✅ **Responsivo**: Adapta-se a diferentes telas
- ✅ **Animações suaves**: Melhor experiência do usuário

## 🔄 Próximos Passos

### Backend
1. Reorganizar controllers em subpastas por módulo
2. Criar DTOs (Data Transfer Objects) para cada módulo
3. Implementar padrão Repository
4. Adicionar validações e filtros
5. Implementar logging estruturado

### Frontend
1. Criar componentes para cada módulo
2. Implementar lazy loading de rotas
3. Criar guards de autenticação
4. Implementar interceptors HTTP
5. Adicionar testes unitários

### Infraestrutura
1. Configurar CI/CD
2. Implementar cache distribuído
3. Adicionar monitoramento
4. Configurar backup automático
5. Implementar versionamento de API

## 📝 Convenções de Código

### Nomenclatura
- **Controllers**: `[Modulo]Controller.cs`
- **Models**: `[Entidade].cs`
- **Services**: `I[Servico]Service.cs` e `[Servico]Service.cs`
- **Components**: `[nome].component.ts`
- **Routes**: `/[modulo]/[funcionalidade]`

### Estrutura de Pastas
- Agrupar por funcionalidade/módulo
- Manter arquivos relacionados próximos
- Separar concerns (models, views, controllers)

### Padrões
- **Backend**: Repository Pattern, Dependency Injection
- **Frontend**: Component-based, Reactive Programming
- **API**: RESTful, versionamento, documentação Swagger

## 🚀 Como Usar

1. **Navegar pelo menu**: Clique nas categorias para expandir/recolher
2. **Buscar funcionalidade**: Use a barra de busca no topo do menu
3. **Acessar módulo**: Clique no item desejado
4. **Personalizar**: Configure temas em Configurações > Temas

## 📚 Documentação Adicional

- [TEMA_PERSONALIZACAO.md](TEMA_PERSONALIZACAO.md) - Sistema de temas
- [TESTE_TEMAS.md](TESTE_TEMAS.md) - Guia de testes
- [README.md](README.md) - Documentação geral
