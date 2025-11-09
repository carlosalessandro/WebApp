-- Script para reorganizar menus por módulos com ícones Bootstrap Icons
-- Execute este script no banco de dados SQLite

-- 1. Desativar menus antigos que não fazem sentido
UPDATE MenuItems SET Ativo = 0 WHERE Id IN (1, 3, 4);

-- 2. DASHBOARD (Menu principal)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Controller, Action, Descricao, EMenuPai, DataCriacao, AbrirNovaAba, MenuPaiId)
VALUES (20, 'Dashboard', 'bi bi-speedometer2', 1, 1, 'Dashboard', 'Index', 'Painel de Controle', 0, datetime('now'), 0, NULL);

-- 3. MÓDULO VENDAS (Menu Pai)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Descricao, EMenuPai, DataCriacao, AbrirNovaAba, MenuPaiId)
VALUES (21, 'Vendas', 'bi bi-cart-check-fill', 2, 1, 'Módulo de Vendas', 1, datetime('now'), 0, NULL);

-- 3.1 PDV (submenu de Vendas)
UPDATE MenuItems 
SET MenuPaiId = 21, Ordem = 1, Icone = 'bi bi-cash-register'
WHERE Id = 10;

-- 3.2 NFC-e (submenu de Vendas)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Controller, Action, Descricao, EMenuPai, MenuPaiId, DataCriacao, AbrirNovaAba)
VALUES (22, 'NFC-e', 'bi bi-receipt-cutoff', 2, 1, 'PDV', 'NFCe', 'Nota Fiscal de Consumidor Eletrônica', 0, 21, datetime('now'), 0);

-- 4. MÓDULO CADASTROS (Menu Pai)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Descricao, EMenuPai, DataCriacao, AbrirNovaAba, MenuPaiId)
VALUES (23, 'Cadastros', 'bi bi-folder-fill', 3, 1, 'Cadastros do Sistema', 1, datetime('now'), 0, NULL);

-- 4.1 Clientes (atualizar existente ID 2)
UPDATE MenuItems 
SET Titulo = 'Clientes', Icone = 'bi bi-people-fill', Ordem = 1, MenuPaiId = 23, EMenuPai = 0, Descricao = 'Gerenciamento de Clientes'
WHERE Id = 2;

-- 4.2 Produtos (atualizar existente ID 13)
UPDATE MenuItems 
SET MenuPaiId = 23, Ordem = 2, Icone = 'bi bi-box-seam-fill'
WHERE Id = 13;

-- 5. TAREFAS (Menu independente)
UPDATE MenuItems 
SET MenuPaiId = NULL, Ordem = 4, Icone = 'bi bi-list-check', EMenuPai = 0
WHERE Id = 14;

-- 6. MÓDULO INTEGRAÇÕES (Menu Pai)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Descricao, EMenuPai, DataCriacao, AbrirNovaAba, MenuPaiId)
VALUES (24, 'Integrações', 'bi bi-plugin', 5, 1, 'Integrações Externas', 1, datetime('now'), 0, NULL);

-- 6.1 WhatsApp (submenu de Integrações)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Controller, Action, Descricao, EMenuPai, MenuPaiId, DataCriacao, AbrirNovaAba)
VALUES (25, 'WhatsApp', 'bi bi-whatsapp', 1, 1, 'WhatsApp', 'Index', 'Integração WhatsApp', 0, 24, datetime('now'), 0);

-- 7. RELATÓRIOS (Menu independente)
UPDATE MenuItems 
SET Ordem = 6, Icone = 'bi bi-graph-up-arrow', EMenuPai = 0, MenuPaiId = NULL
WHERE Id = 19;

-- 8. MÓDULO ADMINISTRAÇÃO (Menu Pai - atualizar existente ID 15)
UPDATE MenuItems 
SET Ordem = 7, Icone = 'bi bi-gear-fill'
WHERE Id = 15;

-- 8.1 Usuários (atualizar existente ID 16)
UPDATE MenuItems 
SET Icone = 'bi bi-people', MenuPaiId = 15
WHERE Id = 16;

-- 8.2 Permissões (atualizar existente ID 17)
UPDATE MenuItems 
SET Icone = 'bi bi-shield-lock-fill', MenuPaiId = 15
WHERE Id = 17;

-- 8.3 Menus (atualizar existente ID 18)
UPDATE MenuItems 
SET Icone = 'bi bi-menu-button-wide-fill', MenuPaiId = 15
WHERE Id = 18;

-- 9. MÓDULO PCP (Menu Pai) - NOVO
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Descricao, EMenuPai, DataCriacao, AbrirNovaAba, MenuPaiId)
VALUES (26, 'PCP', 'bi bi-gear-wide-connected', 8, 1, 'Planejamento e Controle de Produção', 1, datetime('now'), 0, NULL);

-- 9.1 Dashboard PCP (submenu de PCP)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Controller, Action, Descricao, EMenuPai, MenuPaiId, DataCriacao, AbrirNovaAba)
VALUES (27, 'Dashboard', 'bi bi-speedometer2', 1, 1, 'PCP', 'Dashboard', 'Dashboard PCP', 0, 26, datetime('now'), 0);

-- 9.2 Ordens de Produção (submenu de PCP)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Controller, Action, Descricao, EMenuPai, MenuPaiId, DataCriacao, AbrirNovaAba)
VALUES (28, 'Ordens de Produção', 'bi bi-clipboard-data', 2, 1, 'PCP', 'OrdemProducao', 'Gerenciamento de Ordens', 0, 26, datetime('now'), 0);

-- 9.3 Apontamentos (submenu de PCP)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Controller, Action, Descricao, EMenuPai, MenuPaiId, DataCriacao, AbrirNovaAba)
VALUES (29, 'Apontamentos', 'bi bi-check-square', 3, 1, 'PCP', 'Apontamento', 'Apontamentos de Produção', 0, 26, datetime('now'), 0);

-- 9.4 Recursos (submenu de PCP)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Controller, Action, Descricao, EMenuPai, MenuPaiId, DataCriacao, AbrirNovaAba)
VALUES (30, 'Recursos', 'bi bi-tools', 4, 1, 'PCP', 'Recursos', 'Gestão de Recursos', 0, 26, datetime('now'), 0);

-- 9.5 Relatórios PCP (submenu de PCP)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Controller, Action, Descricao, EMenuPai, MenuPaiId, DataCriacao, AbrirNovaAba)
VALUES (31, 'Relatórios', 'bi bi-file-bar-graph', 5, 1, 'RelatorioPCP', 'Index', 'Relatórios do PCP', 0, 26, datetime('now'), 0);

-- 9.6 Construtor de Relatórios (submenu de PCP)
INSERT OR REPLACE INTO MenuItems 
(Id, Titulo, Icone, Ordem, Ativo, Controller, Action, Descricao, EMenuPai, MenuPaiId, DataCriacao, AbrirNovaAba)
VALUES (32, 'Construir Relatório', 'bi bi-tools', 6, 1, 'RelatorioPCP', 'Builder', 'Construtor Drag & Drop', 0, 26, datetime('now'), 0);

-- Verificar resultados
SELECT Id, Titulo, Icone, Ordem, MenuPaiId, EMenuPai, Ativo 
FROM MenuItems 
WHERE Ativo = 1
ORDER BY Ordem, MenuPaiId NULLS FIRST, Id;
