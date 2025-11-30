using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddScrumModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sprints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Objetivo = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    VelocidadeAlvo = table.Column<int>(type: "INTEGER", nullable: true),
                    VelocidadeReal = table.Column<int>(type: "INTEGER", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SprintReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SprintId = table.Column<int>(type: "INTEGER", nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    DataReview = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Facilitador = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Participantes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    PontosPositivos = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    PontosNegativos = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    AcoesDeImelhoria = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    NotaGeral = table.Column<int>(type: "INTEGER", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SprintReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SprintReviews_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CriteriosAceitacao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    StoryPoints = table.Column<int>(type: "INTEGER", nullable: true),
                    Prioridade = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Responsavel = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Epic = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Tags = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SprintId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStories_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskUserStories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Responsavel = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    HorasEstimadas = table.Column<int>(type: "INTEGER", nullable: true),
                    HorasRealizadas = table.Column<int>(type: "INTEGER", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserStoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskUserStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskUserStories_UserStories_UserStoryId",
                        column: x => x.UserStoryId,
                        principalTable: "UserStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9235));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9238));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9239));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9241));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9198));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9203));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9206));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9209));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9269));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9277));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9278));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9281));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9283));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9285));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9287));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9036));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 21, 53, 11, 79, DateTimeKind.Local).AddTicks(9039));

            migrationBuilder.CreateIndex(
                name: "IX_SprintReviews_SprintId",
                table: "SprintReviews",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskUserStories_UserStoryId",
                table: "TaskUserStories",
                column: "UserStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_SprintId",
                table: "UserStories",
                column: "SprintId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SprintReviews");

            migrationBuilder.DropTable(
                name: "TaskUserStories");

            migrationBuilder.DropTable(
                name: "UserStories");

            migrationBuilder.DropTable(
                name: "Sprints");

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4753));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4755));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4757));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4759));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4721));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4723));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4725));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4727));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4793));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4845));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4846));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4848));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4850));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4852));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4854));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4856));

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4858));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4513));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 9, 22, 40, 25, 806, DateTimeKind.Local).AddTicks(4515));
        }
    }
}
