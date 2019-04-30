using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication8.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "linkedTeamname",
                table: "PlayerPerformance",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "linkedplayername",
                table: "PlayerPerformance",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "linkedTeamname",
                table: "PlayerPerformance");

            migrationBuilder.DropColumn(
                name: "linkedplayername",
                table: "PlayerPerformance");
        }
    }
}
