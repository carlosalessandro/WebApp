# Angular Frontend - Sistema Kanban

Este é o frontend Angular para o Sistema Kanban com tema verde-louro.

## 🎨 Tema de Cores

O projeto utiliza um esquema de cores verde-louro (lime/yellow-green):
- **Primary Lime**: #ccff00
- **Primary Green**: #9acd32  
- **Dark Green**: #6b8e23
- **Light Green**: #e6ff99
- **Hover Green**: #b3e600

## 📋 Pré-requisitos

- Node.js (versão 18 ou superior)
- npm (versão 9 ou superior)
- Angular CLI (versão 17 ou superior)

## 🚀 Instalação

1. Navegue até o diretório do ClientApp:
```bash
cd ClientApp
```

2. Instale as dependências:
```bash
npm install
```

3. Se o Angular CLI não estiver instalado globalmente, instale-o:
```bash
npm install -g @angular/cli@17
```

## 💻 Desenvolvimento

Para executar o servidor de desenvolvimento:

```bash
npm start
# ou
ng serve
```

O aplicativo estará disponível em `http://localhost:4200/`

O servidor de desenvolvimento se reconectará automaticamente ao backend ASP.NET Core através do proxy configurado.

## 🏗️ Build para Produção

Para compilar o projeto para produção:

```bash
npm run build
# ou
ng build
```

Os arquivos compilados serão salvos em `../wwwroot/dist/`

## 📦 Estrutura do Projeto

```
ClientApp/
├── src/
│   ├── app/
│   │   ├── components/
│   │   │   ├── kanban/          # Componente Kanban Board
│   │   │   ├── sidebar/         # Menu lateral
│   │   │   └── tarefa-list/     # Lista de tarefas
│   │   ├── models/              # Models TypeScript
│   │   ├── services/            # Serviços HTTP
│   │   ├── app.component.ts     # Componente raiz
│   │   └── app.routes.ts        # Rotas da aplicação
│   ├── assets/                  # Recursos estáticos
│   ├── index.html               # HTML principal
│   ├── main.ts                  # Bootstrap da aplicação
│   └── styles.css               # Estilos globais (tema verde-louro)
├── angular.json                 # Configuração Angular
├── package.json                 # Dependências npm
└── tsconfig.json               # Configuração TypeScript
```

## 🎯 Funcionalidades

### Kanban Board
- ✅ Drag & Drop entre colunas
- ✅ 4 status: A Fazer, Em Progresso, Em Revisão, Concluída
- ✅ Indicadores de prioridade (Baixa, Média, Alta, Crítica)
- ✅ Datas de vencimento com alertas
- ✅ Tags e responsáveis
- ✅ Cores personalizadas por tarefa

### Lista de Tarefas
- ✅ Visualização em tabela
- ✅ Filtros e ordenação
- ✅ Ações rápidas (Ver, Editar, Excluir)

### Design
- ✅ Tema verde-louro consistente
- ✅ Responsivo (mobile-first)
- ✅ Animações suaves
- ✅ Ícones Bootstrap Icons

## 🔌 Integração com Backend

O frontend Angular se comunica com o backend ASP.NET Core através dos seguintes endpoints:

- `GET /Tarefa/GetAll` - Obter todas as tarefas
- `POST /Tarefa/UpdateStatus` - Atualizar status da tarefa
- `POST /Tarefa/Create` - Criar nova tarefa
- `POST /Tarefa/Edit/{id}` - Editar tarefa
- `POST /Tarefa/Delete/{id}` - Excluir tarefa

O proxy está configurado em `proxy.conf.json` para redirecionar requisições `/api` para o backend.

## 📝 Próximos Passos

Para usar a aplicação Angular em produção:

1. Build o projeto Angular: `npm run build`
2. Os arquivos serão copiados para `wwwroot/dist`
3. Configure o ASP.NET Core para servir os arquivos estáticos
4. Adicione fallback para SPA routing no `Program.cs`

## 🎨 Customização de Cores

Para alterar o esquema de cores, edite as variáveis CSS em `src/styles.css`:

```css
:root {
  --primary-green: #9acd32;
  --primary-lime: #ccff00;
  --dark-green: #6b8e23;
  /* ... outras variáveis */
}
```

## 🐛 Troubleshooting

### Erro de CORS
Se encontrar erros de CORS, certifique-se de que o backend está configurado para aceitar requisições do Angular:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

### Porta já em uso
Se a porta 4200 estiver em uso, você pode especificar outra:

```bash
ng serve --port 4201
```

## 📄 Licença

Este projeto faz parte do sistema WebApp e segue a mesma licença.
