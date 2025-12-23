# 🔄 Guia de Migração - Reorganização do Projeto

## 📋 Visão Geral

Este guia descreve como migrar o projeto da estrutura antiga (flat) para a nova estrutura modular organizada por domínio de negócio.

## ⚠️ Antes de Começar

1. **Faça backup do projeto**
2. **Commit todas as alterações pendentes**
3. **Certifique-se de que o projeto está compilando**
4. **Teste a aplicação antes da migração**

## 🚀 Passo a Passo

### Opção 1: Migração Automática (Recomendado)

#### 1. Executar Script de Reorganização

```powershell
# No diretório raiz do projeto
.\Scripts\ReorganizeControllers.ps1
```

Este script irá:
- Criar as subpastas em `Controllers/`
- Mover os controllers para suas respectivas pastas
- Manter os arquivos originais intactos

#### 2. Atualizar Namespaces

```powershell
.\Scripts\UpdateNamespaces.ps1
```

Este script irá:
- Atualizar os namespaces de todos os controllers movidos
- Adicionar o nome da pasta ao namespace (ex: `WebApp.Controllers.Cadastros`)

#### 3. Compilar e Testar

```bash
dotnet build
dotnet test
```

### Opção 2: Migração Manual

Se preferir fazer manualmente ou se os scripts não funcionarem:

#### 1. Criar Estrutura de Pastas

```bash
# Criar pastas para cada módulo
mkdir Controllers/Account
mkdir Controllers/Cadastros
mkdir Controllers/Vendas
mkdir Controllers/Estoque
mkdir Controllers/Financeiro
mkdir Controllers/Compras
mkdir Controllers/Producao
mkdir Controllers/CRM
mkdir Controllers/ERP
mkdir Controllers/Projetos
mkdir Controllers/Ferramentas
mkdir Controllers/Relatorios
mkdir Controllers/Configuracoes
mkdir Controllers/Dashboard
mkdir Controllers/Shared
```

#### 2. Mover Controllers

Mova cada controller para sua pasta correspondente:

**Cadastros:**
```bash
move Controllers/ClienteController.cs Controllers/Cadastros/
move Controllers/FornecedorController.cs Controllers/Cadastros/
move Controllers/ProdutoController.cs Controllers/Cadastros/
```

**Vendas:**
```bash
move Controllers/PDVController.cs Controllers/Vendas/
```

**Financeiro:**
```bash
move Controllers/FinanceiroController.cs Controllers/Financeiro/
```

**Produção:**
```bash
move Controllers/PCPController.cs Controllers/Producao/
move Controllers/RelatorioPCPController.cs Controllers/Producao/
```

**Projetos:**
```bash
move Controllers/TarefaController.cs Controllers/Projetos/
move Controllers/ScrumController.cs Controllers/Projetos/
```

**Ferramentas:**
```bash
move Controllers/NoCodeController.cs Controllers/Ferramentas/
move Controllers/SqlBuilderController.cs Controllers/Ferramentas/
move Controllers/QueryBuilderController.cs Controllers/Ferramentas/
move Controllers/ExcelChatbotController.cs Controllers/Ferramentas/
move Controllers/WhatsAppController.cs Controllers/Ferramentas/
move Controllers/DiagramController.cs Controllers/Ferramentas/
```

**Configurações:**
```bash
move Controllers/UserController.cs Controllers/Configuracoes/
move Controllers/PermissaoController.cs Controllers/Configuracoes/
move Controllers/MenuController.cs Controllers/Configuracoes/
move Controllers/ThemeController.cs Controllers/Configuracoes/
```

**CRM:**
```bash
move Controllers/CRMController.cs Controllers/CRM/
```

**ERP:**
```bash
move Controllers/ERPController.cs Controllers/ERP/
```

**Outros:**
```bash
move Controllers/AccountController.cs Controllers/Account/
move Controllers/EstoqueController.cs Controllers/Estoque/
move Controllers/ComprasController.cs Controllers/Compras/
move Controllers/RelatorioController.cs Controllers/Relatorios/
move Controllers/DashboardController.cs Controllers/Dashboard/
move Controllers/HomeController.cs Controllers/Shared/
move Controllers/TestController.cs Controllers/Shared/
```

#### 3. Atualizar Namespaces Manualmente

Para cada controller movido, atualize o namespace:

**Antes:**
```csharp
namespace WebApp.Controllers
{
    public class ClienteController : ControllerBase
    {
        // ...
    }
}
```

**Depois:**
```csharp
namespace WebApp.Controllers.Cadastros
{
    public class ClienteController : ControllerBase
    {
        // ...
    }
}
```

## 🔍 Verificações Pós-Migração

### 1. Compilação

```bash
dotnet build
```

Verifique se não há erros de compilação.

### 2. Rotas da API

As rotas devem continuar funcionando normalmente:
- `/api/cliente` → `Controllers/Cadastros/ClienteController.cs`
- `/api/pdv` → `Controllers/Vendas/PDVController.cs`
- `/api/theme` → `Controllers/Configuracoes/ThemeController.cs`

### 3. Testes

```bash
dotnet test
```

Execute todos os testes para garantir que nada quebrou.

### 4. Frontend

Verifique se o frontend continua funcionando:

```bash
cd ClientApp
npm start
```

Teste as principais funcionalidades:
- ✅ Login
- ✅ Menu lateral
- ✅ Navegação entre páginas
- ✅ Chamadas à API

## 🐛 Problemas Comuns

### Erro: "Type or namespace not found"

**Causa:** Namespace não atualizado ou referência incorreta.

**Solução:**
1. Verifique o namespace do controller
2. Adicione `using` statements necessários
3. Recompile o projeto

### Erro: "Route not found"

**Causa:** Rota da API não está sendo encontrada.

**Solução:**
1. Verifique o atributo `[Route]` no controller
2. Certifique-se de que o controller está na pasta correta
3. Reinicie a aplicação

### Erro: "Cannot find module"

**Causa:** Imports no frontend não foram atualizados.

**Solução:**
1. Verifique os imports nos componentes Angular
2. Atualize os caminhos se necessário
3. Limpe o cache: `npm cache clean --force`

## 📝 Checklist de Migração

- [ ] Backup do projeto criado
- [ ] Scripts executados ou migração manual concluída
- [ ] Namespaces atualizados
- [ ] Projeto compila sem erros
- [ ] Testes passando
- [ ] Frontend funcionando
- [ ] Rotas da API testadas
- [ ] Menu lateral atualizado
- [ ] Documentação atualizada
- [ ] Commit das alterações

## 🔄 Rollback

Se algo der errado, você pode reverter:

```bash
# Restaurar do backup
git checkout .

# Ou reverter commit específico
git revert <commit-hash>
```

## 📚 Próximos Passos

Após a migração bem-sucedida:

1. **Reorganizar Models** seguindo a mesma estrutura
2. **Criar DTOs** para cada módulo
3. **Implementar Services** organizados por módulo
4. **Atualizar Views** seguindo a estrutura modular
5. **Criar componentes Angular** para cada módulo

## 💡 Dicas

- Migre um módulo por vez para facilitar debugging
- Teste após cada migração de módulo
- Mantenha o backup até ter certeza que tudo funciona
- Documente qualquer problema encontrado
- Atualize a documentação conforme necessário

## 🆘 Suporte

Se encontrar problemas:

1. Verifique os logs de erro
2. Consulte a documentação do ASP.NET Core
3. Revise o [ESTRUTURA_PROJETO.md](ESTRUTURA_PROJETO.md)
4. Verifique se todos os namespaces estão corretos

## ✅ Validação Final

Execute este checklist para validar a migração:

```bash
# 1. Compilar
dotnet build

# 2. Executar testes
dotnet test

# 3. Iniciar aplicação
dotnet run

# 4. Testar endpoints
curl http://localhost:5000/api/theme
curl http://localhost:5000/api/cliente
curl http://localhost:5000/api/pdv

# 5. Testar frontend
cd ClientApp
npm start
```

Se todos os passos acima funcionarem, a migração foi bem-sucedida! 🎉
