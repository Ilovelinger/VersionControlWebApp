using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication8.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "penalty",
                table: "NewMatches",
                newName: "Penalty");

            migrationBuilder.RenameColumn(
                name: "overtime",
                table: "NewMatches",
                newName: "Overtime");

            migrationBuilder.AddColumn<int>(
                name: "RelatedTeamteamId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    teamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    teamName = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.teamId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RelatedTeamteamId",
                table: "AspNetUsers",
                column: "RelatedTeamteamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Team_RelatedTeamteamId",
                table: "AspNetUsers",
                column: "RelatedTeamteamId",
                principalTable: "Team",
                principalColumn: "teamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Team_RelatedTeamteamId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RelatedTeamteamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RelatedTeamteamId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Penalty",
                table: "NewMatches",
                newName: "penalty");

            migrationBuilder.RenameColumn(
                name: "Overtime",
                table: "NewMatches",
                newName: "overtime");
        }
    }
}
