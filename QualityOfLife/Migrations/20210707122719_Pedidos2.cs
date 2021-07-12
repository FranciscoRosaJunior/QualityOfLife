using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityOfLife.Migrations
{
    public partial class Pedidos2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProfissionalId",
                table: "Pedido",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_ProfissionalId",
                table: "Pedido",
                column: "ProfissionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Profissional_ProfissionalId",
                table: "Pedido",
                column: "ProfissionalId",
                principalTable: "Profissional",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Profissional_ProfissionalId",
                table: "Pedido");

            migrationBuilder.DropIndex(
                name: "IX_Pedido_ProfissionalId",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "ProfissionalId",
                table: "Pedido");
        }
    }
}
