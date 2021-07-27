using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityOfLife.Migrations
{
    public partial class PedidoAtualização : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamento",
                table: "Pedido",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FormaPagamento",
                table: "Pedido",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocalPagamento",
                table: "Pedido",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Pagamento",
                table: "Pedido",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReciboEmitido",
                table: "Pedido",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataPagamento",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "FormaPagamento",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "LocalPagamento",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "Pagamento",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "ReciboEmitido",
                table: "Pedido");
        }
    }
}
