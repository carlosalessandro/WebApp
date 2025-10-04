using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Controller = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Action = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissoes_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPermissoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    PermissaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Concedida = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataConcessao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Comentario = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPermissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioPermissoes_Permissoes_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPermissoes_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Ativa", "DataAtualizacao", "DataCriacao", "Descricao", "Nome", "Ordem" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6277), "Permissões relacionadas à administração do sistema", "Administração", 1 },
                    { 2, true, null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6279), "Permissões relacionadas ao gerenciamento de usuários", "Usuários", 2 },
                    { 3, true, null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6325), "Permissões relacionadas ao gerenciamento de clientes", "Clientes", 3 },
                    { 4, true, null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6327), "Permissões relacionadas à visualização de relatórios", "Relatórios", 4 }
                });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6241));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6245));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6248));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6250));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6139));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6141));

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Action", "Ativa", "CategoriaId", "Codigo", "Controller", "DataAtualizacao", "DataCriacao", "Descricao", "Nome", "Ordem" },
                values: new object[,]
                {
                    { 1, null, true, 1, "SISTEMA_GERENCIAR", null, null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6348), "Permissão para gerenciar configurações do sistema", "Gerenciar Sistema", 1 },
                    { 2, "Index", true, 1, "MENU_GERENCIAR", "Menu", null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6351), "Permissão para gerenciar itens do menu", "Gerenciar Menu", 2 },
                    { 3, null, true, 2, "USUARIO_VISUALIZAR", null, null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6352), "Permissão para visualizar lista de usuários", "Visualizar Usuários", 1 },
                    { 4, null, true, 2, "USUARIO_CRIAR", null, null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6354), "Permissão para criar novos usuários", "Criar Usuários", 2 },
                    { 5, null, true, 2, "USUARIO_EDITAR", null, null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6356), "Permissão para editar usuários existentes", "Editar Usuários", 3 },
                    { 6, null, true, 2, "USUARIO_EXCLUIR", null, null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6357), "Permissão para excluir usuários", "Excluir Usuários", 4 },
                    { 7, "Index", true, 3, "CLIENTE_VISUALIZAR", "Cliente", null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6359), "Permissão para visualizar lista de clientes", "Visualizar Clientes", 1 },
                    { 8, "Create", true, 3, "CLIENTE_CRIAR", "Cliente", null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6361), "Permissão para criar novos clientes", "Criar Clientes", 2 },
                    { 9, "Edit", true, 3, "CLIENTE_EDITAR", "Cliente", null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6363), "Permissão para editar clientes existentes", "Editar Clientes", 3 },
                    { 10, "Delete", true, 3, "CLIENTE_EXCLUIR", "Cliente", null, new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6365), "Permissão para excluir clientes", "Excluir Clientes", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Ativa",
                table: "Categorias",
                column: "Ativa");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Ordem",
                table: "Categorias",
                column: "Ordem");

            migrationBuilder.CreateIndex(
                name: "IX_Permissoes_Ativa",
                table: "Permissoes",
                column: "Ativa");

            migrationBuilder.CreateIndex(
                name: "IX_Permissoes_CategoriaId",
                table: "Permissoes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissoes_Codigo",
                table: "Permissoes",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissoes_Ordem",
                table: "Permissoes",
                column: "Ordem");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPermissoes_PermissaoId",
                table: "UsuarioPermissoes",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPermissoes_UsuarioId_PermissaoId",
                table: "UsuarioPermissoes",
                columns: new[] { "UsuarioId", "PermissaoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioPermissoes");

            migrationBuilder.DropTable(
                name: "Permissoes");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1701));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1704));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1707));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1708));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1609));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1611));
        }
    }
}
