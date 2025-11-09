-- Script para Verificar e Corrigir Menu
-- Execute este script para diagnosticar problemas no menu

-- 1. Ver todos os menus ativos
SELECT '=== MENUS ATIVOS ===' as Info;
SELECT Id, Titulo, Icone, Ordem, MenuPaiId, EMenuPai, Ativo, Controller, Action
FROM MenuItems 
WHERE Ativo = 1
ORDER BY COALESCE(MenuPaiId, 0), Ordem, Id;

-- 2. Ver menus inativos (podem estar escondidos)
SELECT '=== MENUS INATIVOS ===' as Info;
SELECT Id, Titulo, Icone, Ordem, MenuPaiId, EMenuPai, Ativo, Controller, Action
FROM MenuItems 
WHERE Ativo = 0
ORDER BY Id;

-- 3. Verificar menus pai sem submenus
SELECT '=== MENUS PAI SEM SUBMENUS ===' as Info;
SELECT p.Id, p.Titulo, p.EMenuPai,
       (SELECT COUNT(*) FROM MenuItems WHERE MenuPaiId = p.Id AND Ativo = 1) as QtdSubmenus
FROM MenuItems p
WHERE p.EMenuPai = 1 AND p.Ativo = 1
HAVING QtdSubmenus = 0;

-- 4. Verificar menus órfãos (MenuPaiId aponta para ID inexistente)
SELECT '=== MENUS ÓRFÃOS ===' as Info;
SELECT m.Id, m.Titulo, m.MenuPaiId
FROM MenuItems m
WHERE m.MenuPaiId IS NOT NULL 
  AND m.MenuPaiId NOT IN (SELECT Id FROM MenuItems)
  AND m.Ativo = 1;

-- 5. Verificar menus duplicados
SELECT '=== MENUS DUPLICADOS ===' as Info;
SELECT Titulo, Controller, Action, COUNT(*) as Quantidade
FROM MenuItems
WHERE Ativo = 1
GROUP BY Titulo, Controller, Action
HAVING COUNT(*) > 1;

-- 6. Verificar menus sem Controller/Action e sem URL
SELECT '=== MENUS SEM DESTINO ===' as Info;
SELECT Id, Titulo, Controller, Action, Url, EMenuPai
FROM MenuItems
WHERE Ativo = 1
  AND EMenuPai = 0
  AND (Controller IS NULL OR Controller = '')
  AND (Action IS NULL OR Action = '')
  AND (Url IS NULL OR Url = '');

-- CORREÇÕES AUTOMÁTICAS

-- Corrigir menus órfãos (remover MenuPaiId inválido)
UPDATE MenuItems 
SET MenuPaiId = NULL
WHERE MenuPaiId IS NOT NULL 
  AND MenuPaiId NOT IN (SELECT Id FROM MenuItems);

-- Ativar todos os menus principais que devem estar visíveis
UPDATE MenuItems SET Ativo = 1 
WHERE Id IN (20, 21, 23, 24, 26) -- Dashboard, Vendas, Cadastros, Integrações, PCP
  AND EMenuPai = 1;

-- Ativar menus importantes que podem estar desativados
UPDATE MenuItems SET Ativo = 1
WHERE Id IN (2, 10, 13, 14, 19) -- Clientes, PDV, Produtos, Tarefas, Relatórios
  AND EMenuPai = 0;

-- Verificar resultado final
SELECT '=== ESTRUTURA FINAL DO MENU ===' as Info;
SELECT 
    CASE WHEN MenuPaiId IS NULL THEN '📁 ' ELSE '   └─ ' END || Titulo as Menu,
    Id,
    Icone,
    Ordem,
    Controller,
    Action,
    CASE WHEN Ativo = 1 THEN '✓' ELSE '✗' END as Ativo,
    CASE WHEN EMenuPai = 1 THEN 'Pai' ELSE 'Item' END as Tipo
FROM MenuItems
WHERE Ativo = 1
ORDER BY COALESCE(MenuPaiId, 0), Ordem, Id;
