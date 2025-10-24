using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNFCeMenuItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adicionar o item de menu para NFC-e
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 15, "NFC-e", null, "bi-receipt", 6, true, false, "Nota Fiscal de Consumidor Eletrônica", "PDV", "Index", null, DateTime.Now, null, false, null }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover o item de menu de NFC-e
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}