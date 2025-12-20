using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1038));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1049));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1051));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1053));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(913));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(924));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(928));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(930));

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "AbrirNovaAba", "Action", "Area", "Ativo", "Controller", "DataAtualizacao", "DataCriacao", "Descricao", "EMenuPai", "Icone", "MenuPaiId", "Ordem", "Titulo", "Url" },
                values: new object[] { 10, false, null, null, true, null, null, new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(939), null, true, "bi-gear", null, 4, "Sistema", null });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1137));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1140));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1147));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1158));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1160));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1166));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1178));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1181));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1183));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(1192));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 518, DateTimeKind.Local).AddTicks(9940));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 15, 20, 47, 518, DateTimeKind.Local).AddTicks(9942));

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "AbrirNovaAba", "Action", "Area", "Ativo", "Controller", "DataAtualizacao", "DataCriacao", "Descricao", "EMenuPai", "Icone", "MenuPaiId", "Ordem", "Titulo", "Url" },
                values: new object[,]
                {
                    { 11, false, "Index", null, true, "CRM", null, new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(941), null, false, "bi-people-fill", 10, 1, "CRM", null },
                    { 12, false, "Index", null, true, "ERP", null, new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(943), null, false, "bi-building", 10, 2, "ERP", null },
                    { 13, false, "Index", null, true, "ExcelChatbot", null, new DateTime(2025, 12, 20, 15, 20, 47, 519, DateTimeKind.Local).AddTicks(946), null, false, "bi-robot", 10, 3, "Chatbot Excel", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1255));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1258));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1259));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1261));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1218));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1221));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1224));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1226));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1303));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1307));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1309));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1310));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1312));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1314));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1316));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1318));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1320));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1322));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1022));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 15, 0, 56, 593, DateTimeKind.Local).AddTicks(1025));
        }
    }
}
