using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartDentAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    IdConsulta = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdPaciente = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataConsulta = table.Column<string>(type: "NVARCHAR2(12)", maxLength: 12, nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.IdConsulta);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    IdPaciente = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NomeCompleto = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    DataNascimento = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    Endereco = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    PlanoOdontologico = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    NumConsultas = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Empresa = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.IdPaciente);
                });

            migrationBuilder.CreateTable(
                name: "Procedimentos",
                columns: table => new
                {
                    IdProcedimento = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdConsulta = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TipoProcedimento = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    Custo = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedimentos", x => x.IdProcedimento);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    IdAlerta = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdPaciente = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TipoAlerta = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    GrauRisco = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Justificativa = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataGeracao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.IdAlerta);
                    table.ForeignKey(
                        name: "FK_Alertas_Pacientes_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_IdPaciente",
                table: "Alertas",
                column: "IdPaciente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Procedimentos");

            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
