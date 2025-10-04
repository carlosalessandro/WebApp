using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Icone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    AbrirNovaAba = table.Column<bool>(type: "INTEGER", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Controller = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Action = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Area = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "AbrirNovaAba", "Action", "Area", "Ativo", "Controller", "DataAtualizacao", "DataCriacao", "Descricao", "Icone", "Ordem", "Titulo", "Url" },
                values: new object[,]
                {
                    { 1, false, "Index", null, true, "Home", null, new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(5197), null, "bi-house", 1, "Home", null },
                    { 2, false, "Index", null, true, "Cliente", null, new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(5205), null, "bi-people", 2, "Clientes", null },
                    { 3, false, "Login", null, true, "Account", null, new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(5207), null, "bi-person", 3, "Login", null },
                    { 4, false, "Privacy", null, true, "Home", null, new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(5209), null, "bi-shield", 4, "Privacidade", null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(4243));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(4245));

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_Ativo",
                table: "MenuItems",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_Ordem",
                table: "MenuItems",
                column: "Ordem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 16, 43, 3, 793, DateTimeKind.Local).AddTicks(5870));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 16, 43, 3, 793, DateTimeKind.Local).AddTicks(5872));
        }
    }
}
