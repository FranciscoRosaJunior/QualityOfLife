using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityOfLife.Migrations
{
    public partial class AtualizacaoPacientesController2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

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
    }
}
