# WebApp – Plataforma de Operações e Kanban

> Aplicação full stack (ASP.NET Core 8 + Angular 17) que unifica módulos de tarefas (Kanban), ERP leve (Clientes, Produtos, Compras, Financeiro, Estoque, PCP) e integrações (PDV, NFC-e, WhatsApp). O objetivo é entregar uma base moderna, segura e extensível com identidade visual verde-louro.

---

## Sumário
1. [Visão Geral](#visão-geral)
2. [Principais Módulos](#principais-módulos)
3. [Stack Tecnológica](#stack-tecnológica)
4. [Arquitetura de Alto Nível](#arquitetura-de-alto-nível)
5. [Pré-requisitos](#pré-requisitos)
6. [Configuração](#configuração)
7. [Executando o Projeto](#executando-o-projeto)
8. [Scripts Úteis](#scripts-úteis)
9. [Estrutura de Pastas](#estrutura-de-pastas)
10. [Banco de Dados & Migrations](#banco-de-dados--migrations)
11. [Testes e Qualidade](#testes-e-qualidade)
12. [Segurança](#segurança)
13. [Troubleshooting](#troubleshooting)
14. [Roadmap](#roadmap)
15. [Contribuição](#contribuição)
16. [Licença](#licença)

---

## Visão Geral
- **Backend:** ASP.NET Core 8 (MVC + API) com Entity Framework Core (SQLite em desenvolvimento / MySQL em produção).
- **Frontend:** Angular 17 (standalone) com tema lime/green compartilhado com Razor Views.
- **Autenticação & Permissões:** Cookies seguros, antifalsificação automática, menu dinâmico filtrado por policies.
- **Observabilidade:** Logging estruturado (ILogger) e cache em memória para componentes críticos (menu, listas).

## Principais Módulos
- **Kanban / Scrum:** Boards drag & drop, backlog, planejamento de sprint.
- **ERP Lite:** Clientes, Produtos, Compras, Estoque, Financeiro, PCP.
- **PDV & NFC-e:** Geração simulada de NFC-e, controle de vendas e integração WhatsApp.
- **Query Builder / Diagramas:** Construção visual de consultas SQL e diagramas para no-code builder.

## Stack Tecnológica
| Camada | Tecnologias |
| --- | --- |
| Frontend | Angular 17, TypeScript, RxJS, Bootstrap 5, Bootstrap Icons |
| Backend | ASP.NET Core 8, EF Core 8, AutoValidateAntiforgery, CORS configurável |
| Banco | SQLite (dev), MySQL 8 (prod via Pomelo) |
| Infra adicional | MemoryCache, HttpClient, serviços customizados (Menu, NFC-e, WhatsApp) |

## Arquitetura de Alto Nível
```
┌─────────────┐      HTTPS       ┌──────────────┐      EF Core       ┌──────────────┐
│ Angular 17  │  <------------> │ ASP.NET Core │  <-------------->  │ Database     │
│ (ClientApp) │  CORS + Cookies │ Controllers  │  DbContext /       │ SQLite/MySQL │
└─────────────┘                 │ + Services   │  Repositories      └──────────────┘
                                └──────────────┘
```
- Rotas REST expostas por controllers (ex.: `/Tarefa`, `/Menu`, `/Financeiro`).
- Menu servido via `MenuViewComponent`, consultando `MenuService` + cache.
- Políticas de segurança aplicadas por `IAuthorizationService` (Controller:Action).

## Pré-requisitos
- .NET SDK 8.0+
- Node.js 18+ e npm 9+
- SQLite (para desenvolvimento) e/ou servidor MySQL 8
- Git 2.40+

## Configuração
1. **Clonar repositório**
   ```bash
   git clone <repo-url>
   cd WebApp
   ```

2. **Variáveis sensíveis**
   - Use **Secret Manager** em dev para connection strings e integrações:
     ```bash
     dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=..."
     ```
   - Em produção utilize Azure Key Vault, AWS Secrets Manager ou variáveis de ambiente.

3. **CORS**
   Configure origens confiáveis em `appsettings.{Environment}.json`:
   ```json
   "Cors": {
     "AllowedOrigins": ["https://app.seudominio.com", "http://localhost:4200"]
   }
   ```

4. **Banco de Dados**
   - Development: SQLite (`Data Source=WebApp.db`).
   - Production: MySQL 8 + Pomelo (`Server=...;Database=...`).

5. **Front-end**
   ```bash
   cd ClientApp
   npm install
   ```

## Executando o Projeto
### Opção A – Apenas backend (Razor)
```bash
dotnet run
# Acesse https://localhost:5001/Account/Login
```

### Opção B – Full stack (Angular + API)
```bash
# Terminal 1 – API
dotnet run

# Terminal 2 – Angular
cd ClientApp
npm start

# Frontend: http://localhost:4200
```

## Scripts Úteis
| Contexto | Comando | Descrição |
| --- | --- | --- |
| API | `dotnet run` | Executa backend com hot reload |
| API | `dotnet watch test` | (Planejado) roda testes unitários/integrados |
| DB | `dotnet ef migrations add <Nome>` | Cria migration |
| DB | `dotnet ef database update` | Aplica migrations |
| Frontend | `npm start` | Servidor dev Angular |
| Frontend | `npm run build` | Build de produção Angular |

## Estrutura de Pastas
```
WebApp/
├── ClientApp/               # Frontend Angular 17
├── Controllers/             # MVC + APIs (Tarefa, Financeiro, PCP, etc.)
├── Models/                  # Entidades EF Core
├── Services/                # Serviços de domínio (Menu, NFC-e, WhatsApp)
├── ViewComponents/          # Componentes Razor (Menu dinâmico)
├── Views/                   # Páginas Razor
├── wwwroot/                 # Assets estáticos (css, js, dist Angular)
├── Migrations/              # Histórico EF Core
├── Program.cs               # Bootstrap (DI, CORS, Auth)
└── README.md                # Este documento
```

## Banco de Dados & Migrations
1. Criar migration:
   ```bash
   dotnet ef migrations add NomeDaMigration
   ```
2. Aplicar migration:
   ```bash
   dotnet ef database update
   ```
3. Resetar base SQLite (opcional): apague `WebApp.db`, `WebApp.db-shm`, `WebApp.db-wal` e rode `update` novamente.

## Testes e Qualidade
- **Unit Tests (planejado):** Cobrir serviços críticos (`MenuService`, `NFCeService`).
- **Integration Tests (planejado):** Validar controllers e pipelines EF.
- **Lint/Analyzers:** Habilitar `dotnet format`, StyleCop/Analyzers, `ng lint` no pipeline.

## Segurança
- Cookies autenticados com `Secure`, `SameSite=None` e `HttpOnly`.
- Antifalsificação automática (`AutoValidateAntiforgeryToken`) + cookie `XSRF-TOKEN` para SPAs.
- CORS configurável por ambiente (`Cors:AllowedOrigins`).
- Políticas de autorização por rota (`Controller:Action`).
- Menu dinâmico filtrado por permissões + cache em memória.
- Senhas com hash SHA256 (sugestão: evoluir para PBKDF2/BCrypt).
- Recomendações adicionais:
  1. Ativar rate limiting / lockout.
  2. Configurar logging estruturado (Serilog/Application Insights).
  3. Habilitar HTTPS obrigatório atrás de proxy reverso.

## Troubleshooting
| Sintoma | Possível causa | Ação |
| --- | --- | --- |
| Erro de CORS | Origem não listada | Atualize `Cors:AllowedOrigins` e reinicie API |
| Menu vazio | Falha em `MenuService` ou políticas | Verifique logs e permissões definidas |
| Drag & Drop não persiste | Endpoint `/Tarefa/UpdateStatus` com erro | Confira console do navegador e logs do servidor |
| NFC-e não gera | Venda inexistente | Garanta registros na tabela `Vendas` |

## Roadmap
1. **Automação de testes:** adicionar projetos `WebApp.Tests` e `ClientApp` unit/E2E.
2. **Autenticação SPA:** JWT + refresh tokens para Angular.
3. **Observabilidade:** dashboards com Application Insights/Grafana.
4. **CI/CD:** pipeline GitHub Actions com build, testes, lint e deploy.
5. **Internacionalização:** suporte i18n pt-BR/en-US.

## Contribuição
1. Crie uma branch (`feat/<feature>` ou `fix/<issue>`).
2. Siga o padrão de commits (`conventional commits` recomendado).
3. Adicione testes e documentação quando necessário.
4. Abra Pull Request descrevendo contexto, abordagem e validações.

## Licença
Uso interno/educacional. Consulte o responsável antes de redistribuir.

---

Made with 💚 seguindo o tema verde-louro e práticas profissionais de desenvolvimento.
