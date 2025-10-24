using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPDVMenuItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adicionar o item de menu para o PDV
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 10, "PDV", null, "bi-cart-check", 5, true, false, "Ponto de Venda", "PDV", "Index", null, DateTime.Now, null, false, null }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover o item de menu do PDV
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}