using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adicionar item de menu para Dashboard
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 11, "Dashboard", null, "bi-speedometer2", 2, true, false, "Painel de Controle", "Dashboard", "Index", null, DateTime.Now, null, false, null }
            );

            // Adicionar menu pai para Cadastros
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 12, "Cadastros", null, "bi-folder", 3, true, false, "Cadastros do Sistema", null, null, null, DateTime.Now, null, true, null }
            );

            // Adicionar item de menu para Produtos
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 13, "Produtos", null, "bi-box", 1, true, false, "Gerenciamento de Produtos", "Produto", "Index", null, DateTime.Now, null, false, 12 }
            );

            // Adicionar item de menu para Tarefas
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 14, "Tarefas", null, "bi-list-check", 2, true, false, "Gerenciamento de Tarefas", "Tarefa", "Index", null, DateTime.Now, null, false, 12 }
            );

            // Adicionar menu pai para Administração
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 15, "Administração", null, "bi-gear", 6, true, false, "Administração do Sistema", null, null, null, DateTime.Now, null, true, null }
            );

            // Adicionar item de menu para Usuários
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 16, "Usuários", null, "bi-people", 1, true, false, "Gerenciamento de Usuários", "User", "Index", null, DateTime.Now, null, false, 15 }
            );

            // Adicionar item de menu para Permissões
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 17, "Permissões", null, "bi-shield-lock", 2, true, false, "Gerenciamento de Permissões", "Permissao", "Index", null, DateTime.Now, null, false, 15 }
            );

            // Adicionar item de menu para Menus
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 18, "Menus", null, "bi-menu-button-wide", 3, true, false, "Gerenciamento de Menus", "Menu", "Index", null, DateTime.Now, null, false, 15 }
            );

            // Adicionar item de menu para Relatórios
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Titulo", "Url", "Icone", "Ordem", "Ativo", "AbrirNovaAba", "Descricao", "Controller", "Action", "Area", "DataCriacao", "DataAtualizacao", "EMenuPai", "MenuPaiId" },
                values: new object[] { 19, "Relatórios", null, "bi-graph-up", 4, true, false, "Relatórios do Sistema", "Relatorio", "Index", null, DateTime.Now, null, false, null }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover os itens de menu adicionados
            for (int i = 11; i <= 19; i++)
            {
                migrationBuilder.DeleteData(
                    table: "MenuItems",
                    keyColumn: "Id",
                    keyValue: i);
            }
        }
    }
}