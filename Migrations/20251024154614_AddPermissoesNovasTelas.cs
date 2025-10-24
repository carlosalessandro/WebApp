using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissoesNovasTelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adicionar permissões para as novas telas
            migrationBuilder.Sql(@"
                INSERT INTO Permissoes (Id, Nome, Descricao, Codigo, Controller, Action, Ativa, CategoriaId, Ordem, DataCriacao)
                SELECT 20, 'Acessar PDV', 'Permite acessar o PDV', 'PDV_ACESSO', 'PDV', 'Index', 1, 1, 1, CURRENT_TIMESTAMP
                WHERE NOT EXISTS (SELECT 1 FROM Permissoes WHERE Nome = 'Acessar PDV');

                INSERT INTO Permissoes (Id, Nome, Descricao, Codigo, Controller, Action, Ativa, CategoriaId, Ordem, DataCriacao)
                SELECT 21, 'Gerenciar Produtos', 'Permite gerenciar produtos', 'PRODUTO_GERENCIAR', 'Produto', 'Index', 1, 1, 2, CURRENT_TIMESTAMP
                WHERE NOT EXISTS (SELECT 1 FROM Permissoes WHERE Nome = 'Gerenciar Produtos');

                INSERT INTO Permissoes (Id, Nome, Descricao, Codigo, Controller, Action, Ativa, CategoriaId, Ordem, DataCriacao)
                SELECT 22, 'Visualizar Relatórios', 'Permite visualizar relatórios', 'RELATORIO_VISUALIZAR', 'Relatorio', 'Index', 1, 1, 3, CURRENT_TIMESTAMP
                WHERE NOT EXISTS (SELECT 1 FROM Permissoes WHERE Nome = 'Visualizar Relatórios');

                INSERT INTO Permissoes (Id, Nome, Descricao, Codigo, Controller, Action, Ativa, CategoriaId, Ordem, DataCriacao)
                SELECT 23, 'Gerenciar Tarefas', 'Permite gerenciar tarefas', 'TAREFA_GERENCIAR', 'Tarefa', 'Index', 1, 1, 4, CURRENT_TIMESTAMP
                WHERE NOT EXISTS (SELECT 1 FROM Permissoes WHERE Nome = 'Gerenciar Tarefas');
            ");
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover as permissões adicionadas usando SQL
            migrationBuilder.Sql(@"
                DELETE FROM Permissoes WHERE Id IN (20, 21, 22, 23);
            ");
                
            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3923));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3925));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3927));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3928));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3884));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3886));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3888));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3890));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3957));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3965));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3967));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3969));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3970));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3972));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3974));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3976));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3978));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3980));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3658));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 12, 30, 13, 549, DateTimeKind.Local).AddTicks(3661));
        }
    }
}
