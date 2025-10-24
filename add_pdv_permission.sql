-- Garantir que a permissão PDV_ACESSO existe
INSERT OR IGNORE INTO Permissoes (Nome, Codigo, DataCriacao)
VALUES ('Acessar PDV', 'PDV_ACESSO', CURRENT_TIMESTAMP);

-- Adicionar a permissão ao usuário administrador (ID 1)
INSERT OR IGNORE INTO UsuarioPermissoes (UsuarioId, PermissaoId)
SELECT 1, Id FROM Permissoes WHERE Codigo = 'PDV_ACESSO';