using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddModuloPCP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdensProducao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroOrdem = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeProduzida = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInicioPrevista = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFimPrevista = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataInicioReal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFimReal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Prioridade = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CriadoPorId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdensProducao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdensProducao_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdensProducao_Users_CriadoPorId",
                        column: x => x.CriadoPorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CapacidadePorHora = table.Column<decimal>(type: "TEXT", nullable: false),
                    CustoPorHora = table.Column<decimal>(type: "TEXT", nullable: false),
                    Disponivel = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmManutencao = table.Column<bool>(type: "INTEGER", nullable: false),
                    Localizacao = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelatoriosPCP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TipoRelatorio = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ConfiguracaoJson = table.Column<string>(type: "TEXT", nullable: true),
                    CamposSelecionados = table.Column<string>(type: "TEXT", nullable: true),
                    Filtros = table.Column<string>(type: "TEXT", nullable: true),
                    Ordenacao = table.Column<string>(type: "TEXT", nullable: true),
                    Agrupamento = table.Column<string>(type: "TEXT", nullable: true),
                    Publico = table.Column<bool>(type: "INTEGER", nullable: false),
                    CriadoPorId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatoriosPCP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatoriosPCP_Users_CriadoPorId",
                        column: x => x.CriadoPorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApontamentosProducao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrdemProducaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeProduzida = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeRefugo = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeRetrabalho = table.Column<int>(type: "INTEGER", nullable: false),
                    DataHoraApontamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OperadorId = table.Column<int>(type: "INTEGER", nullable: true),
                    TempoProducaoMinutos = table.Column<int>(type: "INTEGER", nullable: false),
                    TempoSetupMinutos = table.Column<int>(type: "INTEGER", nullable: false),
                    TempoParadoMinutos = table.Column<int>(type: "INTEGER", nullable: false),
                    MotivoParada = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApontamentosProducao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApontamentosProducao_OrdensProducao_OrdemProducaoId",
                        column: x => x.OrdemProducaoId,
                        principalTable: "OrdensProducao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApontamentosProducao_Users_OperadorId",
                        column: x => x.OperadorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecursosAlocados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrdemProducaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecursoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInicioAlocacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFimAlocacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HorasPlanejadas = table.Column<decimal>(type: "TEXT", nullable: false),
                    HorasUtilizadas = table.Column<decimal>(type: "TEXT", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursosAlocados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecursosAlocados_OrdensProducao_OrdemProducaoId",
                        column: x => x.OrdemProducaoId,
                        principalTable: "OrdensProducao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecursosAlocados_Recursos_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "Recursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8534));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8538));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8541));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8493));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8498));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8502));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8504));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8577));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8585));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8587));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8589));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8591));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8593));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8595));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8597));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8599));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8601));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8208));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 7, 8, 33, 7, 665, DateTimeKind.Local).AddTicks(8211));

            migrationBuilder.CreateIndex(
                name: "IX_ApontamentosProducao_OperadorId",
                table: "ApontamentosProducao",
                column: "OperadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ApontamentosProducao_OrdemProducaoId",
                table: "ApontamentosProducao",
                column: "OrdemProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensProducao_CriadoPorId",
                table: "OrdensProducao",
                column: "CriadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensProducao_ProdutoId",
                table: "OrdensProducao",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosAlocados_OrdemProducaoId",
                table: "RecursosAlocados",
                column: "OrdemProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosAlocados_RecursoId",
                table: "RecursosAlocados",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatoriosPCP_CriadoPorId",
                table: "RelatoriosPCP",
                column: "CriadoPorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApontamentosProducao");

            migrationBuilder.DropTable(
                name: "RecursosAlocados");

            migrationBuilder.DropTable(
                name: "RelatoriosPCP");

            migrationBuilder.DropTable(
                name: "OrdensProducao");

            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1065));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1067));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1069));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1070));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1027));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1032));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1035));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1037));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1099));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1103));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1105));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1106));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1108));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1110));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1112));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1114));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1116));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(1117));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(890));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 20, 4, 34, 951, DateTimeKind.Local).AddTicks(892));
        }
    }
}
