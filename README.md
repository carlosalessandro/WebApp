# WebApp - SQLite com Entity Framework Core

Este projeto implementa SQLite com Entity Framework Core em uma aplicação ASP.NET Core.

## Funcionalidades Implementadas

- ✅ SQLite como banco de dados
- ✅ Entity Framework Core para ORM
- ✅ Sistema de autenticação com hash de senhas
- ✅ Registro de usuários
- ✅ Login de usuários
- ✅ Migrations para controle de versão do banco

## Estrutura do Projeto

### Modelos (Models)
- `LoginModel.cs` - Modelo para login/registro
- `User.cs` - Entidade de usuário para o banco de dados

### Dados (Data)
- `ApplicationDbContext.cs` - Contexto do Entity Framework Core

### Controladores (Controllers)
- `AccountController.cs` - Controlador para autenticação e registro

### Views
- `Views/Account/Login.cshtml` - Página de login
- `Views/Account/Register.cshtml` - Página de registro

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

## Como Usar

1. **Registrar um usuário**: Acesse `/Account/Register`
2. **Fazer login**: Acesse `/Account/Login`
3. **Fazer logout**: Acesse `/Account/Logout`

## Tecnologias Utilizadas

- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQLite
- Bootstrap (para UI)
- jQuery (para validação)

## Segurança

- Senhas são hasheadas usando SHA256
- Validação de entrada com Data Annotations
- Proteção CSRF com AntiForgeryToken
- Logs de auditoria para login/logout
