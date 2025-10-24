using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<UsuarioPermissao> UsuarioPermissoes { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoImagem> ProdutoImagens { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<WhatsAppIntegracao> WhatsAppIntegracoes { get; set; }
        public DbSet<NFCe> NFCes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações do modelo User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).IsRequired();

                // Índice único para email
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configurações do modelo Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Endereco).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(14);
                entity.Property(e => e.EstadoCivil).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Bairro).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Telefone).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Cidade).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DataCadastro).IsRequired();

                // Índice único para CPF
                entity.HasIndex(e => e.CPF).IsUnique();
                // Índice único para email
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configurações do modelo MenuItem
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Url).HasMaxLength(200);
                entity.Property(e => e.Icone).HasMaxLength(50);
                entity.Property(e => e.Descricao).HasMaxLength(500);
                entity.Property(e => e.Controller).HasMaxLength(50);
                entity.Property(e => e.Action).HasMaxLength(50);
                entity.Property(e => e.Area).HasMaxLength(50);
                entity.Property(e => e.DataCriacao).IsRequired();

                // Índice para ordenação
                entity.HasIndex(e => e.Ordem);
                // Índice para itens ativos
                entity.HasIndex(e => e.Ativo);

                // Configuração do relacionamento hierárquico
                entity.HasOne(e => e.MenuPai)
                      .WithMany(e => e.SubMenus)
                      .HasForeignKey(e => e.MenuPaiId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurações do modelo Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(500);
                entity.Property(e => e.DataCriacao).IsRequired();
                entity.HasIndex(e => e.Ativa);
                entity.HasIndex(e => e.Ordem);
            });

            // Configurações do modelo Permissao
            modelBuilder.Entity<Permissao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(200);
                entity.Property(e => e.Codigo).HasMaxLength(50);
                entity.Property(e => e.Controller).HasMaxLength(50);
                entity.Property(e => e.Action).HasMaxLength(50);
                entity.Property(e => e.DataCriacao).IsRequired();
                entity.HasIndex(e => e.Ativa);
                entity.HasIndex(e => e.Ordem);
                entity.HasIndex(e => e.Codigo).IsUnique();

                // Relacionamento com Categoria
                entity.HasOne(e => e.Categoria)
                      .WithMany(e => e.Permissoes)
                      .HasForeignKey(e => e.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurações do modelo UsuarioPermissao
            modelBuilder.Entity<UsuarioPermissao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DataConcessao).IsRequired();
                entity.Property(e => e.Comentario).HasMaxLength(200);

                // Relacionamento com User
                entity.HasOne(e => e.Usuario)
                      .WithMany(e => e.UsuarioPermissoes)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relacionamento com Permissao
                entity.HasOne(e => e.Permissao)
                      .WithMany(e => e.UsuarioPermissoes)
                      .HasForeignKey(e => e.PermissaoId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice único para evitar duplicatas
                entity.HasIndex(e => new { e.UsuarioId, e.PermissaoId }).IsUnique();
            });

            // Configurações do modelo Tarefa
            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descricao).HasMaxLength(1000);
                entity.Property(e => e.Responsavel).HasMaxLength(100);
                entity.Property(e => e.Tags).HasMaxLength(200);
                entity.Property(e => e.Cor).HasMaxLength(7);
                entity.Property(e => e.DataCriacao).IsRequired();
                
                // Índices para performance
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Prioridade);
                entity.HasIndex(e => e.Ordem);
                entity.HasIndex(e => e.DataVencimento);
                entity.HasIndex(e => e.Responsavel);
            });

            // Configurações do modelo Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descricao).HasMaxLength(1000);
                entity.Property(e => e.Preco).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.PrecoCusto).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Codigo).HasMaxLength(50);
                entity.Property(e => e.Marca).HasMaxLength(100);
                entity.Property(e => e.Modelo).HasMaxLength(100);
                entity.Property(e => e.Cor).HasMaxLength(50);
                entity.Property(e => e.Tamanho).HasMaxLength(20);
                entity.Property(e => e.Peso).HasMaxLength(100);
                entity.Property(e => e.Dimensoes).HasMaxLength(50);
                entity.Property(e => e.DataCriacao).IsRequired();
                
                // Índices para performance
                entity.HasIndex(e => e.Nome);
                entity.HasIndex(e => e.Codigo).IsUnique();
                entity.HasIndex(e => e.Ativo);
                entity.HasIndex(e => e.Destaque);
                entity.HasIndex(e => e.CategoriaId);
                
                // Relacionamento com Categoria
                entity.HasOne(e => e.Categoria)
                      .WithMany()
                      .HasForeignKey(e => e.CategoriaId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configurações do modelo ProdutoImagem
            modelBuilder.Entity<ProdutoImagem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NomeArquivo).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Caminho).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Titulo).HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(200);
                entity.Property(e => e.DataCriacao).IsRequired();
                
                // Índices para performance
                entity.HasIndex(e => e.ProdutoId);
                entity.HasIndex(e => e.Principal);
                entity.HasIndex(e => e.Ordem);
                
                // Relacionamento com Produto
                entity.HasOne(e => e.Produto)
                      .WithMany(e => e.Imagens)
                      .HasForeignKey(e => e.ProdutoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Dados iniciais para teste
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@teste.com",
                    Password = "123456", // Em produção, usar hash da senha
                    Name = "Administrador",
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    Email = "usuario@teste.com",
                    Password = "123456", // Em produção, usar hash da senha
                    Name = "Usuário Teste",
                    CreatedAt = DateTime.Now
                }
            );

            // Dados iniciais do menu
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Titulo = "Home",
                    Controller = "Home",
                    Action = "Index",
                    Icone = "bi-house",
                    Ordem = 1,
                    Ativo = true,
                    DataCriacao = DateTime.Now
                },
                new MenuItem
                {
                    Id = 2,
                    Titulo = "Cadastro",
                    Icone = "bi-folder",
                    Ordem = 2,
                    Ativo = true,
                    EMenuPai = true,
                    DataCriacao = DateTime.Now
                },
                new MenuItem
                {
                    Id = 3,
                    Titulo = "Cliente",
                    Controller = "Cliente",
                    Action = "Index",
                    Icone = "bi-people",
                    Ordem = 1,
                    Ativo = true,
                    MenuPaiId = 2,
                    DataCriacao = DateTime.Now
                },
                new MenuItem
                {
                    Id = 4,
                    Titulo = "Privacidade",
                    Controller = "Home",
                    Action = "Privacy",
                    Icone = "bi-shield",
                    Ordem = 3,
                    Ativo = true,
                    DataCriacao = DateTime.Now
                }
            );

            // Dados iniciais das categorias
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria
                {
                    Id = 1,
                    Nome = "Administração",
                    Descricao = "Permissões relacionadas à administração do sistema",
                    Ordem = 1,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Categoria
                {
                    Id = 2,
                    Nome = "Usuários",
                    Descricao = "Permissões relacionadas ao gerenciamento de usuários",
                    Ordem = 2,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Categoria
                {
                    Id = 3,
                    Nome = "Clientes",
                    Descricao = "Permissões relacionadas ao gerenciamento de clientes",
                    Ordem = 3,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Categoria
                {
                    Id = 4,
                    Nome = "Relatórios",
                    Descricao = "Permissões relacionadas à visualização de relatórios",
                    Ordem = 4,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                }
            );

            // Dados iniciais das permissões
            modelBuilder.Entity<Permissao>().HasData(
                // Permissões de Administração
                new Permissao
                {
                    Id = 1,
                    Nome = "Gerenciar Sistema",
                    Descricao = "Permissão para gerenciar configurações do sistema",
                    Codigo = "SISTEMA_GERENCIAR",
                    CategoriaId = 1,
                    Ordem = 1,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Permissao
                {
                    Id = 2,
                    Nome = "Gerenciar Menu",
                    Descricao = "Permissão para gerenciar itens do menu",
                    Codigo = "MENU_GERENCIAR",
                    Controller = "Menu",
                    Action = "Index",
                    CategoriaId = 1,
                    Ordem = 2,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                // Permissões de Usuários
                new Permissao
                {
                    Id = 3,
                    Nome = "Visualizar Usuários",
                    Descricao = "Permissão para visualizar lista de usuários",
                    Codigo = "USUARIO_VISUALIZAR",
                    CategoriaId = 2,
                    Ordem = 1,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Permissao
                {
                    Id = 4,
                    Nome = "Criar Usuários",
                    Descricao = "Permissão para criar novos usuários",
                    Codigo = "USUARIO_CRIAR",
                    CategoriaId = 2,
                    Ordem = 2,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Permissao
                {
                    Id = 5,
                    Nome = "Editar Usuários",
                    Descricao = "Permissão para editar usuários existentes",
                    Codigo = "USUARIO_EDITAR",
                    CategoriaId = 2,
                    Ordem = 3,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Permissao
                {
                    Id = 6,
                    Nome = "Excluir Usuários",
                    Descricao = "Permissão para excluir usuários",
                    Codigo = "USUARIO_EXCLUIR",
                    CategoriaId = 2,
                    Ordem = 4,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                // Permissões de Clientes
                new Permissao
                {
                    Id = 7,
                    Nome = "Visualizar Clientes",
                    Descricao = "Permissão para visualizar lista de clientes",
                    Codigo = "CLIENTE_VISUALIZAR",
                    Controller = "Cliente",
                    Action = "Index",
                    CategoriaId = 3,
                    Ordem = 1,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Permissao
                {
                    Id = 8,
                    Nome = "Criar Clientes",
                    Descricao = "Permissão para criar novos clientes",
                    Codigo = "CLIENTE_CRIAR",
                    Controller = "Cliente",
                    Action = "Create",
                    CategoriaId = 3,
                    Ordem = 2,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Permissao
                {
                    Id = 9,
                    Nome = "Editar Clientes",
                    Descricao = "Permissão para editar clientes existentes",
                    Codigo = "CLIENTE_EDITAR",
                    Controller = "Cliente",
                    Action = "Edit",
                    CategoriaId = 3,
                    Ordem = 3,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new Permissao
                {
                    Id = 10,
                    Nome = "Excluir Clientes",
                    Descricao = "Permissão para excluir clientes",
                    Codigo = "CLIENTE_EXCLUIR",
                    Controller = "Cliente",
                    Action = "Delete",
                    CategoriaId = 3,
                    Ordem = 4,
                    Ativa = true,
                    DataCriacao = DateTime.Now
                }
            );
        }
    }
}
