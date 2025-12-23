# Script para reorganizar Controllers em subpastas por módulo

$controllerMappings = @{
    # Account
    "AccountController.cs" = "Account"
    
    # Cadastros
    "ClienteController.cs" = "Cadastros"
    "FornecedorController.cs" = "Cadastros"
    "ProdutoController.cs" = "Cadastros"
    
    # Vendas
    "PDVController.cs" = "Vendas"
    
    # Estoque
    "EstoqueController.cs" = "Estoque"
    
    # Financeiro
    "FinanceiroController.cs" = "Financeiro"
    
    # Compras
    "ComprasController.cs" = "Compras"
    
    # Producao
    "PCPController.cs" = "Producao"
    "RelatorioPCPController.cs" = "Producao"
    
    # CRM
    "CRMController.cs" = "CRM"
    
    # ERP
    "ERPController.cs" = "ERP"
    
    # Projetos
    "TarefaController.cs" = "Projetos"
    "ScrumController.cs" = "Projetos"
    
    # Ferramentas
    "NoCodeController.cs" = "Ferramentas"
    "SqlBuilderController.cs" = "Ferramentas"
    "QueryBuilderController.cs" = "Ferramentas"
    "SqlJoinDemoController.cs" = "Ferramentas"
    "ExcelChatbotController.cs" = "Ferramentas"
    "WhatsAppController.cs" = "Ferramentas"
    "DiagramController.cs" = "Ferramentas"
    
    # Relatorios
    "RelatorioController.cs" = "Relatorios"
    
    # Configuracoes
    "UserController.cs" = "Configuracoes"
    "PermissaoController.cs" = "Configuracoes"
    "UsuarioPermissaoController.cs" = "Configuracoes"
    "MenuController.cs" = "Configuracoes"
    "ThemeController.cs" = "Configuracoes"
    
    # Dashboard
    "DashboardController.cs" = "Dashboard"
    
    # Shared
    "HomeController.cs" = "Shared"
    "TestController.cs" = "Shared"
}

Write-Host "🔄 Reorganizando Controllers..." -ForegroundColor Cyan

# Criar diretórios
$folders = $controllerMappings.Values | Select-Object -Unique
foreach ($folder in $folders) {
    $path = "Controllers/$folder"
    if (-not (Test-Path $path)) {
        New-Item -ItemType Directory -Path $path -Force | Out-Null
        Write-Host "✅ Criado: $path" -ForegroundColor Green
    }
}

# Mover arquivos
foreach ($file in $controllerMappings.Keys) {
    $sourcePath = "Controllers/$file"
    $targetFolder = $controllerMappings[$file]
    $targetPath = "Controllers/$targetFolder/$file"
    
    if (Test-Path $sourcePath) {
        Move-Item -Path $sourcePath -Destination $targetPath -Force
        Write-Host "📦 Movido: $file -> $targetFolder/" -ForegroundColor Yellow
    } else {
        Write-Host "⚠️  Não encontrado: $file" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "✨ Reorganização concluída!" -ForegroundColor Green
Write-Host ""
Write-Host "⚠️  IMPORTANTE: Você precisará atualizar os namespaces nos arquivos movidos!" -ForegroundColor Yellow
Write-Host "   Exemplo: namespace WebApp.Controllers.Cadastros" -ForegroundColor Gray
