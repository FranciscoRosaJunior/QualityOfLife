using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityOfLife.Migrations
{
    public partial class CamposAgenda2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diariamente",
                table: "Agenda");

            migrationBuilder.DropColumn(
                name: "Mensalmente",
                table: "Agenda");

            migrationBuilder.DropColumn(
                name: "Semanalmente",
                table: "Agenda");

            migrationBuilder.AddColumn<int>(
                name: "Repetir",
                table: "Agenda",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Repetir",
                table: "Agenda");

            migrationBuilder.AddColumn<bool>(
                name: "Diariamente",
                table: "Agenda",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Mensalmente",
                table: "Agenda",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Semanalmente",
                table: "Agenda",
                nullable: false,
                defaultValue: false);
        }
    }
}
