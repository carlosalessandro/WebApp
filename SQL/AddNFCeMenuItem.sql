-- Adicionar o item de menu para NFC-e
INSERT INTO MenuItems (Id, Titulo, Url, Icone, Ordem, Ativo, AbrirNovaAba, Descricao, Controller, Action, Area, DataCriacao, EMenuPai, MenuPaiId)
VALUES (15, 'NFC-e', NULL, 'bi-receipt', 6, 1, 0, 'Nota Fiscal de Consumidor Eletrônica', 'PDV', 'Index', NULL, datetime('now'), 0, NULL);

-- Adicionar permissão para acessar NFC-e
INSERT INTO Permissoes (Nome, Descricao, Chave)
VALUES ('Acesso a NFC-e', 'Permite acesso às funcionalidades de NFC-e', 'NFCe');

-- Adicionar permissão ao usuário admin (assumindo que o ID 1 é o admin)
INSERT INTO UsuarioPermissoes (UsuarioId, PermissaoId)
SELECT 1, p.Id FROM Permissoes p WHERE p.Chave = 'NFCe';