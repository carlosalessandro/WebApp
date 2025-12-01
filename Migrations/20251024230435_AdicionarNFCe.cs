using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarNFCe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChaveAcessoNFCe",
                table: "Vendas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEmissaoNFCe",
                table: "Vendas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NFCeId",
                table: "Vendas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroNFCe",
                table: "Vendas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "PossuiNFCe",
                table: "Vendas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SerieNFCe",
                table: "Vendas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StatusNFCe",
                table: "Vendas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "NFCes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    Serie = table.Column<string>(type: "TEXT", nullable: false),
                    ChaveAcesso = table.Column<string>(type: "TEXT", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Xml = table.Column<string>(type: "TEXT", nullable: false),
                    ProtocoloAutorizacao = table.Column<string>(type: "TEXT", nullable: false),
                    DataAutorizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MensagemRetorno = table.Column<string>(type: "TEXT", nullable: false),
                    UrlConsulta = table.Column<string>(type: "TEXT", nullable: false),
                    Ambiente = table.Column<string>(type: "TEXT", nullable: false),
                    TipoEmissao = table.Column<string>(type: "TEXT", nullable: false),
                    VendaId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFCes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NFCes_Vendas_VendaId",
                        column: x => x.VendaId,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppIntegracoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TokenAcesso = table.Column<string>(type: "TEXT", nullable: false),
                    NumeroTelefone = table.Column<string>(type: "TEXT", nullable: false),
                    BusinessAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    WebhookUrl = table.Column<string>(type: "TEXT", nullable: false),
                    WebhookSecret = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppIntegracoes", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_NFCeId",
                table: "Vendas",
                column: "NFCeId");

            migrationBuilder.CreateIndex(
                name: "IX_NFCes_VendaId",
                table: "NFCes",
                column: "VendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_NFCes_NFCeId",
                table: "Vendas",
                column: "NFCeId",
                principalTable: "NFCes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_NFCes_NFCeId",
                table: "Vendas");

            migrationBuilder.DropTable(
                name: "NFCes");

            migrationBuilder.DropTable(
                name: "WhatsAppIntegracoes");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_NFCeId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "ChaveAcessoNFCe",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "DataEmissaoNFCe",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "NFCeId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "NumeroNFCe",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "PossuiNFCe",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "SerieNFCe",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "StatusNFCe",
                table: "Vendas");

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(811));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(816));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(818));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(821));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(745));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(751));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(756));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(759));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(875));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(881));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(884));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(886));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(889));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(892));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(895));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(897));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(900));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(903));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(337));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 12, 46, 14, 298, DateTimeKind.Local).AddTicks(342));
        }
    }
}
