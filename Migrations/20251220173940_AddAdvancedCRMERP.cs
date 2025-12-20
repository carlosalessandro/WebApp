using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvancedCRMERP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampanhasMarketing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Orcamento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvestimentoAtual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PublicoAlvo = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Mensagem = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    CriadoPorId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampanhasMarketing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampanhasMarketing_Users_CriadoPorId",
                        column: x => x.CriadoPorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    GerenteId = table.Column<int>(type: "INTEGER", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departamentos_Users_GerenteId",
                        column: x => x.GerenteId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrdensProducaoCompletas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroOP = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadePlanejada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantidadeProduzida = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantidadeDefeitos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Prioridade = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    GerenteProducaoId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataEmissao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataInicioPlanejada = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFimPlanejada = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataInicioReal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFimReal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CustoEstimado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustoReal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: true),
                    PedidoVendaId = table.Column<int>(type: "INTEGER", nullable: true),
                    EspecificacoesTecnicas = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdensProducaoCompletas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdensProducaoCompletas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrdensProducaoCompletas_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdensProducaoCompletas_Users_GerenteProducaoId",
                        column: x => x.GerenteProducaoId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrigensLeads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrigensLeads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanoContas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    PaiId = table.Column<int>(type: "INTEGER", nullable: true),
                    Nivel = table.Column<int>(type: "INTEGER", nullable: false),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    Analitica = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClassificacaoFiscal = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoContas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoContas_PlanoContas_PaiId",
                        column: x => x.PaiId,
                        principalTable: "PlanoContas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Prioridade = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFimPrevista = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFimReal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Orcamento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustoAtual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GerenteId = table.Column<int>(type: "INTEGER", nullable: true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projetos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projetos_Users_GerenteId",
                        column: x => x.GerenteId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecursosProducao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CapacidadePorHora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustoPorHora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Localizacao = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UltimaManutencao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProximaManutencao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursosProducao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AtividadesCampanha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CampanhaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DataAgendamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataExecucao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Resultado = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Custo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ContatosAlcancados = table.Column<int>(type: "INTEGER", nullable: true),
                    ConversoesGeradas = table.Column<int>(type: "INTEGER", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadesCampanha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtividadesCampanha_CampanhasMarketing_CampanhaId",
                        column: x => x.CampanhaId,
                        principalTable: "CampanhasMarketing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CentrosCusto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    GerenteId = table.Column<int>(type: "INTEGER", nullable: true),
                    DepartamentoId = table.Column<int>(type: "INTEGER", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    OrcamentoAnual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrosCusto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentrosCusto_Departamentos_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamentos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CentrosCusto_Users_GerenteId",
                        column: x => x.GerenteId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InspecoesQualidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrdemProducaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInspecao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InspetorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeInspecionada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantidadeAprovada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantidadeRejeitada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Resultado = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    DefeitosEncontrados = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Liberada = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataLiberacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspecoesQualidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspecoesQualidade_OrdensProducaoCompletas_OrdemProducaoId",
                        column: x => x.OrdemProducaoId,
                        principalTable: "OrdensProducaoCompletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspecoesQualidade_Users_InspetorId",
                        column: x => x.InspetorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Empresa = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Cargo = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    OrigemId = table.Column<int>(type: "INTEGER", nullable: false),
                    CampanhaId = table.Column<int>(type: "INTEGER", nullable: true),
                    ResponsavelId = table.Column<int>(type: "INTEGER", nullable: true),
                    ValorEstimado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataPrimeiroContato = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataUltimoContato = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataConversao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leads_CampanhasMarketing_CampanhaId",
                        column: x => x.CampanhaId,
                        principalTable: "CampanhasMarketing",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leads_OrigensLeads_OrigemId",
                        column: x => x.OrigemId,
                        principalTable: "OrigensLeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leads_Users_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApontamentosProducaoCompletos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrdemProducaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecursoId = table.Column<int>(type: "INTEGER", nullable: false),
                    GerenteProducaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    OperadorId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataHoraInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataHoraFim = table.Column<DateTime>(type: "TEXT", nullable: true),
                    QuantidadeProduzida = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantidadeDefeitos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApontamentosProducaoCompletos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApontamentosProducaoCompletos_OrdensProducaoCompletas_OrdemProducaoId",
                        column: x => x.OrdemProducaoId,
                        principalTable: "OrdensProducaoCompletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApontamentosProducaoCompletos_RecursosProducao_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "RecursosProducao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApontamentosProducaoCompletos_Users_GerenteProducaoId",
                        column: x => x.GerenteProducaoId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApontamentosProducaoCompletos_Users_OperadorId",
                        column: x => x.OperadorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecursosAlocadosOP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrdemProducaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecursoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataAlocacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataDesalocacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PercentualAlocacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursosAlocadosOP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecursosAlocadosOP_OrdensProducaoCompletas_OrdemProducaoId",
                        column: x => x.OrdemProducaoId,
                        principalTable: "OrdensProducaoCompletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecursosAlocadosOP_RecursosProducao_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "RecursosProducao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LancamentosContabeis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroDocumento = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DataLancamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Historico = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContaDebitoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContaCreditoId = table.Column<int>(type: "INTEGER", nullable: false),
                    CentroCustoId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjetoId = table.Column<int>(type: "INTEGER", nullable: true),
                    CriadoPorId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DataAprovacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AprovadoPorId = table.Column<int>(type: "INTEGER", nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentosContabeis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LancamentosContabeis_CentrosCusto_CentroCustoId",
                        column: x => x.CentroCustoId,
                        principalTable: "CentrosCusto",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LancamentosContabeis_PlanoContas_ContaCreditoId",
                        column: x => x.ContaCreditoId,
                        principalTable: "PlanoContas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentosContabeis_PlanoContas_ContaDebitoId",
                        column: x => x.ContaDebitoId,
                        principalTable: "PlanoContas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentosContabeis_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LancamentosContabeis_Users_AprovadoPorId",
                        column: x => x.AprovadoPorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LancamentosContabeis_Users_CriadoPorId",
                        column: x => x.CriadoPorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeadsCampanha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CampanhaId = table.Column<int>(type: "INTEGER", nullable: false),
                    LeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataAssociacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrigemAssociacao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Convertido = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataConversao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadsCampanha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadsCampanha_CampanhasMarketing_CampanhaId",
                        column: x => x.CampanhaId,
                        principalTable: "CampanhasMarketing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadsCampanha_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Oportunidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    LeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ResponsavelId = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorEstimado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProbabilidadeSucesso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFechamentoPrevista = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFechamentoReal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProdutosServicos = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oportunidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oportunidades_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Oportunidades_Users_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AtividadesCRM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DataAgendamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ResponsavelId = table.Column<int>(type: "INTEGER", nullable: false),
                    LeadId = table.Column<int>(type: "INTEGER", nullable: true),
                    OportunidadeId = table.Column<int>(type: "INTEGER", nullable: true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: true),
                    Resultado = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    LeadEspecificoId = table.Column<int>(type: "INTEGER", nullable: true),
                    OportunidadeEspecificaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadesCRM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtividadesCRM_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AtividadesCRM_Leads_LeadEspecificoId",
                        column: x => x.LeadEspecificoId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtividadesCRM_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AtividadesCRM_Oportunidades_OportunidadeEspecificaId",
                        column: x => x.OportunidadeEspecificaId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtividadesCRM_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AtividadesCRM_Users_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostasComerciais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    OportunidadeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataValidade = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataAceite = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TermosCondicoes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CriadoPorId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostasComerciais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostasComerciais_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostasComerciais_Users_CriadoPorId",
                        column: x => x.CriadoPorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItensProposta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PropostaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unidade = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensProposta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensProposta_PropostasComerciais_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "PropostasComerciais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1544));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1546));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1551));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1568));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1063));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1079));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1082));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1088));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1753));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1769));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1771));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1781));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1783));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1787));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1801));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1822));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1827));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 987, DateTimeKind.Local).AddTicks(1840));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 12, 20, 14, 39, 39, 986, DateTimeKind.Local).AddTicks(9352), "admin123" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 14, 39, 39, 986, DateTimeKind.Local).AddTicks(9355));

            migrationBuilder.CreateIndex(
                name: "IX_ApontamentosProducaoCompletos_GerenteProducaoId",
                table: "ApontamentosProducaoCompletos",
                column: "GerenteProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ApontamentosProducaoCompletos_OperadorId",
                table: "ApontamentosProducaoCompletos",
                column: "OperadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ApontamentosProducaoCompletos_OrdemProducaoId",
                table: "ApontamentosProducaoCompletos",
                column: "OrdemProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ApontamentosProducaoCompletos_RecursoId",
                table: "ApontamentosProducaoCompletos",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCampanha_CampanhaId",
                table: "AtividadesCampanha",
                column: "CampanhaId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCRM_ClienteId",
                table: "AtividadesCRM",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCRM_LeadEspecificoId",
                table: "AtividadesCRM",
                column: "LeadEspecificoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCRM_LeadId",
                table: "AtividadesCRM",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCRM_OportunidadeEspecificaId",
                table: "AtividadesCRM",
                column: "OportunidadeEspecificaId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCRM_OportunidadeId",
                table: "AtividadesCRM",
                column: "OportunidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCRM_ResponsavelId",
                table: "AtividadesCRM",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_CampanhasMarketing_CriadoPorId",
                table: "CampanhasMarketing",
                column: "CriadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_CentrosCusto_DepartamentoId",
                table: "CentrosCusto",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_CentrosCusto_GerenteId",
                table: "CentrosCusto",
                column: "GerenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_GerenteId",
                table: "Departamentos",
                column: "GerenteId");

            migrationBuilder.CreateIndex(
                name: "IX_InspecoesQualidade_InspetorId",
                table: "InspecoesQualidade",
                column: "InspetorId");

            migrationBuilder.CreateIndex(
                name: "IX_InspecoesQualidade_OrdemProducaoId",
                table: "InspecoesQualidade",
                column: "OrdemProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensProposta_PropostaId",
                table: "ItensProposta",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosContabeis_AprovadoPorId",
                table: "LancamentosContabeis",
                column: "AprovadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosContabeis_CentroCustoId",
                table: "LancamentosContabeis",
                column: "CentroCustoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosContabeis_ContaCreditoId",
                table: "LancamentosContabeis",
                column: "ContaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosContabeis_ContaDebitoId",
                table: "LancamentosContabeis",
                column: "ContaDebitoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosContabeis_CriadoPorId",
                table: "LancamentosContabeis",
                column: "CriadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosContabeis_ProjetoId",
                table: "LancamentosContabeis",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CampanhaId",
                table: "Leads",
                column: "CampanhaId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_OrigemId",
                table: "Leads",
                column: "OrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ResponsavelId",
                table: "Leads",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadsCampanha_CampanhaId",
                table: "LeadsCampanha",
                column: "CampanhaId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadsCampanha_LeadId",
                table: "LeadsCampanha",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Oportunidades_LeadId",
                table: "Oportunidades",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Oportunidades_ResponsavelId",
                table: "Oportunidades",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensProducaoCompletas_ClienteId",
                table: "OrdensProducaoCompletas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensProducaoCompletas_GerenteProducaoId",
                table: "OrdensProducaoCompletas",
                column: "GerenteProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensProducaoCompletas_ProdutoId",
                table: "OrdensProducaoCompletas",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoContas_PaiId",
                table: "PlanoContas",
                column: "PaiId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_ClienteId",
                table: "Projetos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_GerenteId",
                table: "Projetos",
                column: "GerenteId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostasComerciais_CriadoPorId",
                table: "PropostasComerciais",
                column: "CriadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostasComerciais_OportunidadeId",
                table: "PropostasComerciais",
                column: "OportunidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosAlocadosOP_OrdemProducaoId",
                table: "RecursosAlocadosOP",
                column: "OrdemProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosAlocadosOP_RecursoId",
                table: "RecursosAlocadosOP",
                column: "RecursoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApontamentosProducaoCompletos");

            migrationBuilder.DropTable(
                name: "AtividadesCampanha");

            migrationBuilder.DropTable(
                name: "AtividadesCRM");

            migrationBuilder.DropTable(
                name: "InspecoesQualidade");

            migrationBuilder.DropTable(
                name: "ItensProposta");

            migrationBuilder.DropTable(
                name: "LancamentosContabeis");

            migrationBuilder.DropTable(
                name: "LeadsCampanha");

            migrationBuilder.DropTable(
                name: "RecursosAlocadosOP");

            migrationBuilder.DropTable(
                name: "PropostasComerciais");

            migrationBuilder.DropTable(
                name: "CentrosCusto");

            migrationBuilder.DropTable(
                name: "PlanoContas");

            migrationBuilder.DropTable(
                name: "Projetos");

            migrationBuilder.DropTable(
                name: "OrdensProducaoCompletas");

            migrationBuilder.DropTable(
                name: "RecursosProducao");

            migrationBuilder.DropTable(
                name: "Oportunidades");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "CampanhasMarketing");

            migrationBuilder.DropTable(
                name: "OrigensLeads");

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9235));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9238));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9239));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9241));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9198));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9203));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9206));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9209));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9269));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9277));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9278));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9281));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9283));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9285));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9287));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9036), "123456" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9039));
        }
    }
}
