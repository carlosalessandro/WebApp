# Script para atualizar namespaces dos Controllers reorganizados

$controllerFolders = @(
    "Account",
    "Cadastros",
    "Vendas",
    "Estoque",
    "Financeiro",
    "Compras",
    "Producao",
    "CRM",
    "ERP",
    "Projetos",
    "Ferramentas",
    "Relatorios",
    "Configuracoes",
    "Dashboard",
    "Shared"
)

Write-Host "🔄 Atualizando namespaces..." -ForegroundColor Cyan

foreach ($folder in $controllerFolders) {
    $path = "Controllers/$folder"
    
    if (Test-Path $path) {
        $files = Get-ChildItem -Path $path -Filter "*.cs"
        
        foreach ($file in $files) {
            $content = Get-Content $file.FullName -Raw
            
            # Atualizar namespace
            $newNamespace = "namespace WebApp.Controllers.$folder"
            $content = $content -replace "namespace WebApp\.Controllers\s*$", $newNamespace
            
            # Salvar arquivo
            Set-Content -Path $file.FullName -Value $content
            
            Write-Host "✅ Atualizado: $folder/$($file.Name)" -ForegroundColor Green
        }
    }
}

Write-Host ""
Write-Host "✨ Namespaces atualizados!" -ForegroundColor Green
