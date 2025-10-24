using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class AddWhatsAppIntegracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhatsAppIntegracoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TokenAcesso = table.Column<string>(type: "TEXT", nullable: false),
                    NumeroTelefone = table.Column<string>(type: "TEXT", nullable: false),
                    BusinessAccountId = table.Column<string>(type: "TEXT", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    WebhookUrl = table.Column<string>(type: "TEXT", nullable: true),
                    WebhookSecret = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppIntegracoes", x => x.Id);
                });

            // Adicionar item de menu para WhatsApp
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Nome", "Descricao", "Url", "Icone", "ParentMenuItemId", "Ordem", "Ativo" },
                values: new object[] { 15, "WhatsApp", "Integração com WhatsApp", "/WhatsApp", "fab fa-whatsapp", null, 8, true });

            // Adicionar permissão para WhatsApp
            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Nome", "Descricao" },
                values: new object[] { 15, "WhatsApp", "Acesso à integração com WhatsApp" });

            // Adicionar permissão ao usuário admin (ID 1)
            migrationBuilder.InsertData(
                table: "UsuarioPermissoes",
                columns: new[] { "Id", "UserId", "PermissaoId" },
                values: new object[] { 15, 1, 15 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhatsAppIntegracoes");

            // Remover permissão do usuário admin
            migrationBuilder.DeleteData(
                table: "UsuarioPermissoes",
                keyColumn: "Id",
                keyValue: 15);

            // Remover permissão
            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 15);

            // Remover item de menu
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}