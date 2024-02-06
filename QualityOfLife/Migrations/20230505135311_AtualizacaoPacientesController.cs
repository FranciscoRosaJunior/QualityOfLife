using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityOfLife.Migrations
{
    public partial class AtualizacaoPacientesController : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PacienteDiaAtendimento_Paciente_PacienteId",
                table: "PacienteDiaAtendimento");

            migrationBuilder.DropIndex(
                name: "IX_PacienteDiaAtendimento_PacienteId",
                table: "PacienteDiaAtendimento");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Paciente");

            migrationBuilder.DropColumn(
                name: "DiaAtendimento",
                table: "Paciente");

            migrationBuilder.AlterColumn<long>(
                name: "PacienteId",
                table: "PacienteDiaAtendimento",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaDaSemana",
                table: "PacienteDiaAtendimento",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_PacienteDiaAtendimento_PacienteId",
                table: "PacienteDiaAtendimento",
                column: "PacienteId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PacienteDiaAtendimento_Paciente_PacienteId",
                table: "PacienteDiaAtendimento",
                column: "PacienteId",
                principalTable: "Paciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PacienteDiaAtendimento_Paciente_PacienteId",
                table: "PacienteDiaAtendimento");

            migrationBuilder.DropIndex(
                name: "IX_PacienteDiaAtendimento_PacienteId",
                table: "PacienteDiaAtendimento");

            migrationBuilder.AlterColumn<long>(
                name: "PacienteId",
                table: "PacienteDiaAtendimento",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "DiaDaSemana",
                table: "PacienteDiaAtendimento",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Paciente",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DiaAtendimento",
                table: "Paciente",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PacienteDiaAtendimento_PacienteId",
                table: "PacienteDiaAtendimento",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_PacienteDiaAtendimento_Paciente_PacienteId",
                table: "PacienteDiaAtendimento",
                column: "PacienteId",
                principalTable: "Paciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
