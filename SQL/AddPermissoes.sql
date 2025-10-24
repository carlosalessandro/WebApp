-- Adicionar permissões para as novas telas
INSERT INTO Permissoes (Id, Nome, Descricao, Codigo, Controller, Action, Ativa, CategoriaId, Ordem, DataCriacao)
SELECT 20, 'Acessar PDV', 'Permite acessar o PDV', 'PDV_ACESSO', 'PDV', 'Index', 1, 1, 1, CURRENT_TIMESTAMP
WHERE NOT EXISTS (SELECT 1 FROM Permissoes WHERE Nome = 'Acessar PDV');

INSERT INTO Permissoes (Id, Nome, Descricao, Codigo, Controller, Action, Ativa, CategoriaId, Ordem, DataCriacao)
SELECT 21, 'Gerenciar Produtos', 'Permite gerenciar produtos', 'PRODUTO_GERENCIAR', 'Produto', 'Index', 1, 1, 2, CURRENT_TIMESTAMP
WHERE NOT EXISTS (SELECT 1 FROM Permissoes WHERE Nome = 'Gerenciar Produtos');

INSERT INTO Permissoes (Id, Nome, Descricao, Codigo, Controller, Action, Ativa, CategoriaId, Ordem, DataCriacao)
SELECT 22, 'Visualizar Relatórios', 'Permite visualizar relatórios', 'RELATORIO_VISUALIZAR', 'Relatorio', 'Index', 1, 1, 3, CURRENT_TIMESTAMP
WHERE NOT EXISTS (SELECT 1 FROM Permissoes WHERE Nome = 'Visualizar Relatórios');

INSERT INTO Permissoes (Id, Nome, Descricao, Codigo, Controller, Action, Ativa, CategoriaId, Ordem, DataCriacao)
SELECT 23, 'Gerenciar Tarefas', 'Permite gerenciar tarefas', 'TAREFA_GERENCIAR', 'Tarefa', 'Index', 1, 1, 4, CURRENT_TIMESTAMP
WHERE NOT EXISTS (SELECT 1 FROM Permissoes WHERE Nome = 'Gerenciar Tarefas');