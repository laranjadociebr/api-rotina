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
                name: "RotinasTarefas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomeTarefa = table.Column<string>(type: "TEXT", nullable: false),
                    TipoTarefa = table.Column<string>(type: "TEXT", nullable: false),
                    DataHorarioInicial = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataHorarioFinal = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiasSemana = table.Column<string>(type: "TEXT", nullable: false),
                    Duracao = table.Column<string>(type: "TEXT", nullable: false),
                    Localizacao = table.Column<string>(type: "TEXT", nullable: false),
                    Alarme = table.Column<string>(type: "TEXT", nullable: false),
                    WidgetAtivado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RotinasTarefas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RotinasTarefas");

            migrationBuilder.DropTable(
                name: "Tarefas");
        }
    }
}
