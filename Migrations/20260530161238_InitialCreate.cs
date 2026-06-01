using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaPrimeiraApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rotinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomeRotina = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rotinas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RotinaId = table.Column<int>(type: "INTEGER", nullable: false),
                    NomeTarefa = table.Column<string>(type: "TEXT", nullable: false),
                    TarefaSelecionada = table.Column<string>(type: "TEXT", nullable: false),
                    HorarioInicio = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    HorarioFim = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DiaSemana = table.Column<string>(type: "TEXT", nullable: false),
                    Duracao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarefas_Rotinas_RotinaId",
                        column: x => x.RotinaId,
                        principalTable: "Rotinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_RotinaId",
                table: "Tarefas",
                column: "RotinaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarefas");

            migrationBuilder.DropTable(
                name: "Rotinas");
        }
    }
}
