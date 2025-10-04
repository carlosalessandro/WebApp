using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EMenuPai",
                table: "MenuItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MenuPaiId",
                table: "MenuItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataCriacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1701), false, null });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Action", "Controller", "DataCriacao", "EMenuPai", "Icone", "MenuPaiId", "Titulo" },
                values: new object[] { null, null, new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1704), true, "bi-folder", null, "Cadastro" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Action", "Controller", "DataCriacao", "EMenuPai", "Icone", "MenuPaiId", "Ordem", "Titulo" },
                values: new object[] { "Index", "Cliente", new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1707), false, "bi-people", 2, 1, "Cliente" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DataCriacao", "EMenuPai", "MenuPaiId", "Ordem" },
                values: new object[] { new DateTime(2025, 9, 28, 10, 22, 4, 147, DateTimeKind.Local).AddTicks(1708), false, null, 3 });

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

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuPaiId",
                table: "MenuItems",
                column: "MenuPaiId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_MenuItems_MenuPaiId",
                table: "MenuItems",
                column: "MenuPaiId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_MenuItems_MenuPaiId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_MenuPaiId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "EMenuPai",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "MenuPaiId",
                table: "MenuItems");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(5197));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Action", "Controller", "DataCriacao", "Icone", "Titulo" },
                values: new object[] { "Index", "Cliente", new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(5205), "bi-people", "Clientes" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Action", "Controller", "DataCriacao", "Icone", "Ordem", "Titulo" },
                values: new object[] { "Login", "Account", new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(5207), "bi-person", 3, "Login" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DataCriacao", "Ordem" },
                values: new object[] { new DateTime(2025, 9, 26, 16, 48, 55, 256, DateTimeKind.Local).AddTicks(5209), 4 });

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
        }
    }
}
