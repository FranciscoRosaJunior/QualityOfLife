using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityOfLife.Migrations
{
    public partial class Pedidos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfissionalPedidos_Pedido_PedidoId",
                table: "ProfissionalPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfissionalPedidos_Paciente_PedidoId1",
                table: "ProfissionalPedidos");

            migrationBuilder.DropIndex(
                name: "IX_ProfissionalPedidos_PedidoId1",
                table: "ProfissionalPedidos");

            migrationBuilder.DropColumn(
                name: "PedidoId1",
                table: "ProfissionalPedidos");

            migrationBuilder.DropColumn(
                name: "DataPrevista",
                table: "Pedido");

            migrationBuilder.RenameColumn(
                name: "NomeProfissional",
                table: "Pedido",
                newName: "Observações");

            migrationBuilder.AddColumn<double>(
                name: "Credito",
                table: "Pedido",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfissionalPedidos_Paciente_PedidoId",
                table: "ProfissionalPedidos",
                column: "PedidoId",
                principalTable: "Paciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfissionalPedidos_Paciente_PedidoId",
                table: "ProfissionalPedidos");

            migrationBuilder.DropColumn(
                name: "Credito",
                table: "Pedido");

            migrationBuilder.RenameColumn(
                name: "Observações",
                table: "Pedido",
                newName: "NomeProfissional");

            migrationBuilder.AddColumn<long>(
                name: "PedidoId1",
                table: "ProfissionalPedidos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPrevista",
                table: "Pedido",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalPedidos_PedidoId1",
                table: "ProfissionalPedidos",
                column: "PedidoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfissionalPedidos_Pedido_PedidoId",
                table: "ProfissionalPedidos",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfissionalPedidos_Paciente_PedidoId1",
                table: "ProfissionalPedidos",
                column: "PedidoId1",
                principalTable: "Paciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
