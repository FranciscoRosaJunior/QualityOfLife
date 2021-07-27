using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityOfLife.Migrations
{
    public partial class AgendaAtualização : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocalAtendimento",
                table: "Agenda",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalAtendimento",
                table: "Agenda");
        }
    }
}
