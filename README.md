# WebApp - Sistema Kanban com Angular + ASP.NET Core

Sistema completo de gerenciamento de tarefas com **Kanban Board** interativo, frontend **Angular 17**, backend **ASP.NET Core 8.0** e tema **verde-louro** (lime/yellow-green).

## 🎨 Características Visuais

### Tema Verde-Louro
- **Cores principais:** #ccff00 (lime), #9acd32 (green), #6b8e23 (dark green)
- **Menu lateral:** Gradiente verde-louro com animações
- **Botões e elementos:** Consistência visual em todo o projeto
- **Interface moderna:** Cards com sombras, transições suaves

## ✨ Funcionalidades Implementadas

### 🎯 Sistema Kanban
- ✅ **Kanban Board Angular** com drag & drop funcional
- ✅ **Kanban Board Razor** (view original, ainda funciona)
- ✅ 4 status de tarefas: A Fazer → Em Progresso → Em Revisão → Concluída
- ✅ Prioridades: Baixa, Média, Alta, Crítica
- ✅ Datas de vencimento com alertas
- ✅ Responsáveis e tags
- ✅ Cores personalizadas por tarefa

### 💻 Frontend Angular
- ✅ Angular 17 standalone components
- ✅ TypeScript com tipos fortemente tipados
- ✅ RxJS para programação reativa
- ✅ Roteamento com lazy loading
- ✅ Serviços HTTP para API
- ✅ Componentes reutilizáveis

### 🔐 Autenticação e Permissões
- ✅ Sistema de autenticação com hash de senhas
- ✅ Registro de usuários
- ✅ Login/Logout
- ✅ Sistema de permissões granular
- ✅ Gestão de menus dinâmica

### 📊 Outros Módulos
- ✅ Gestão de clientes
- ✅ Catálogo de produtos
- ✅ PDV (Ponto de Venda)
- ✅ Integração WhatsApp
- ✅ Emissão de NFC-e
- ✅ Relatórios e dashboards

### 💾 Banco de Dados
- ✅ SQLite com Entity Framework Core
- ✅ Migrations para versionamento
- ✅ Relacionamentos entre entidades

## 📁 Estrutura do Projeto

```
WebApp/
├── ClientApp/                          # 🅰️ Frontend Angular 17
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/
│   │   │   │   ├── kanban/            # Kanban Board interativo
│   │   │   │   ├── sidebar/           # Menu lateral
│   │   │   │   └── tarefa-list/       # Lista de tarefas
│   │   │   ├── models/                # TypeScript models
│   │   │   ├── services/              # HTTP services
│   │   │   └── app.routes.ts          # Rotas Angular
│   │   ├── styles.css                 # Tema verde-louro global
│   │   └── index.html
│   └── package.json
│
├── Controllers/                        # 🎮 Backend ASP.NET Core
│   ├── TarefaController.cs            # API + Views Kanban
│   ├── AccountController.cs           # Autenticação
│   ├── ClienteController.cs           # Gestão de clientes
│   ├── ProdutoController.cs           # Catálogo de produtos
│   ├── PDVController.cs               # Ponto de Venda
│   └── ...
│
├── Models/                             # 📊 Entidades do banco
│   ├── Tarefa.cs                      # Modelo de tarefa
│   ├── User.cs                        # Usuário
│   ├── Cliente.cs                     # Cliente
│   └── ...
│
├── Views/                              # 🎨 Razor Views (Razor MVC)
│   ├── Tarefa/
│   │   ├── Kanban.cshtml              # Kanban Razor (original)
│   │   └── Index.cshtml
│   ├── Shared/
│   │   └── _Layout.cshtml             # Layout com tema verde-louro
│   └── ...
│
├── wwwroot/                            # 📦 Arquivos estáticos
│   ├── css/
│   │   └── site.css                   # CSS global com tema
│   ├── js/
│   └── dist/                          # Build do Angular
│
├── Services/                           # 🔧 Serviços backend
├── Migrations/                         # 🗄️ Entity Framework Migrations
└── Program.cs                          # ⚙️ Configuração (CORS, etc.)
```

## Configuração do Banco de Dados

### Connection String
O banco SQLite está configurado no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=WebApp.db"
  }
}
```

### Migrations
Para criar uma nova migration:
```bash
dotnet ef migrations add NomeDaMigration
```

Para aplicar as migrations:
```bash
dotnet ef database update
```

## 🚀 Como Usar

### Opção 1: Apenas Backend (Razor Views)

1. Navegue até a pasta do projeto:
```bash
cd c:\Users\Alessandro\source\repos\WebApp
```

2. Execute o projeto:
```bash
dotnet run
```

3. Acesse no navegador:
- Login: `https://localhost:5001/Account/Login`
- Kanban (Razor): `https://localhost:5001/Tarefa/Kanban`

### Opção 2: Angular + Backend (Recomendado)

**Primeira vez - Instalar dependências:**

1. Instale Node.js 18+ (se não tiver): https://nodejs.org/

2. Navegue até a pasta Angular:
```bash
cd c:\Users\Alessandro\source\repos\WebApp\ClientApp
```

3. Instale as dependências:
```bash
npm install
```

**Executando:**

**Terminal 1 - Backend:**
```bash
cd c:\Users\Alessandro\source\repos\WebApp
dotnet run
```

**Terminal 2 - Frontend Angular:**
```bash
cd c:\Users\Alessandro\source\repos\WebApp\ClientApp
npm start
```

4. Acesse: `http://localhost:4200`

📖 **Guia completo de instalação:** Consulte `INSTALACAO_ANGULAR.md`

## 🛠️ Tecnologias Utilizadas

### Frontend
- **Angular 17** - Framework web moderno
- **TypeScript** - JavaScript tipado
- **RxJS** - Programação reativa
- **Bootstrap 5** - Framework CSS
- **Bootstrap Icons** - Ícones

### Backend
- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core 8.0** - ORM
- **SQLite** - Banco de dados
- **CORS** - Cross-Origin Resource Sharing

### Design
- **Tema Verde-Louro** - Paleta personalizada
- **CSS Variables** - Customização fácil
- **Animações CSS** - Transições suaves

## 🔒 Segurança

- Senhas são hasheadas usando SHA256
- Validação de entrada com Data Annotations
- Proteção CSRF com AntiForgeryToken
- Logs de auditoria para login/logout
- CORS configurado para Angular
- Sistema de permissões granular

## 🎯 Funcionalidades do Kanban

### Drag & Drop
- Arraste tarefas entre colunas
- Atualização automática no banco
- Feedback visual durante o arraste

### Informações da Tarefa
- Título e descrição
- Status atual
- Prioridade (cores: verde, amarelo, vermelho, preto)
- Data de vencimento (com alerta se atrasada)
- Responsável
- Tags personalizadas
- Cor da borda personalizável

### Ações
- Ver detalhes
- Editar tarefa
- Excluir tarefa
- Mudar status por drag & drop

## 📋 API Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/Tarefa/GetAll` | Lista todas as tarefas (JSON para Angular) |
| GET | `/Tarefa/Kanban` | View Razor do Kanban |
| POST | `/Tarefa/UpdateStatus` | Atualiza status da tarefa |
| POST | `/Tarefa/Create` | Cria nova tarefa |
| POST | `/Tarefa/Edit/{id}` | Edita tarefa |
| POST | `/Tarefa/Delete/{id}` | Exclui tarefa |

## 🎨 Customização do Tema

Para alterar as cores verde-louro, edite as variáveis CSS:

**Angular:** `ClientApp/src/styles.css`
**Razor:** `wwwroot/css/site.css`

```css
:root {
  --primary-green: #9acd32;    /* Verde principal */
  --primary-lime: #ccff00;     /* Lima/Amarelo-esverdeado */
  --dark-green: #6b8e23;       /* Verde escuro */
  --light-green: #e6ff99;      /* Verde claro */
  --hover-green: #b3e600;      /* Verde hover */
  --text-dark: #1a3309;        /* Texto escuro */
  --text-medium: #2d5016;      /* Texto médio */
}
```

## 📚 Documentação Adicional

- **Instalação Angular:** `INSTALACAO_ANGULAR.md` - Guia completo
- **README Angular:** `ClientApp/README.md` - Específico do frontend
- **Migrations:** Ver pasta `Migrations/` para histórico do banco

## 🐛 Problemas Conhecidos

### Drag & Drop no Kanban
Se o drag & drop não funcionar:
1. Verifique se o endpoint `/Tarefa/UpdateStatus` está respondendo
2. Abra o console do navegador (F12) para ver erros
3. Teste a view Razor em `/Tarefa/Kanban` para comparação

### CORS
Se houver erro de CORS entre Angular e Backend:
- Certifique-se que o backend está rodando
- Verifique a configuração em `Program.cs`
- A origem `http://localhost:4200` deve estar permitida

## 🚀 Próximos Passos

Sugestões de melhorias:

1. **CRUD Completo no Angular**
   - Formulários de criação/edição
   - Modais de confirmação

2. **Filtros e Busca**
   - Filtrar por prioridade
   - Buscar por texto
   - Filtrar por responsável

3. **Notificações**
   - Toasts para ações
   - Alertas de tarefas atrasadas

4. **Autenticação no Angular**
   - Guards para rotas
   - Interceptors HTTP
   - JWT tokens

5. **Testes**
   - Unit tests (Angular + .NET)
   - Integration tests
   - E2E tests

## 👥 Contribuindo

Para contribuir com o projeto:
1. Fork o repositório
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## 📄 Licença

Este projeto é de uso interno/educacional.
