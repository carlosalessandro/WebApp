using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTarefaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Prioridade = table.Column<int>(type: "INTEGER", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Responsavel = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EstimativaHoras = table.Column<decimal>(type: "TEXT", nullable: true),
                    TempoGasto = table.Column<decimal>(type: "TEXT", nullable: true),
                    Tags = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Cor = table.Column<string>(type: "TEXT", maxLength: 7, nullable: true),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5684));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5686));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5688));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5689));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5633));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5635));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5639));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5640));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5886));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5889));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5891));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5893));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5895));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5896));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5898));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5900));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5902));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5904));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5446));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 20, 7, 28, 408, DateTimeKind.Local).AddTicks(5448));

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_DataVencimento",
                table: "Tarefas",
                column: "DataVencimento");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_Ordem",
                table: "Tarefas",
                column: "Ordem");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_Prioridade",
                table: "Tarefas",
                column: "Prioridade");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_Responsavel",
                table: "Tarefas",
                column: "Responsavel");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_Status",
                table: "Tarefas",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarefas");

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6277));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6279));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6325));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6327));

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
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6348));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6351));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6352));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6354));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6356));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6357));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6359));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6361));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6363));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 9, 28, 11, 8, 53, 887, DateTimeKind.Local).AddTicks(6365));

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
        }
    }
}
