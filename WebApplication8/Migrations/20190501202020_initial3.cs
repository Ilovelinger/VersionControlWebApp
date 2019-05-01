using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication8.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelatedUserId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RelatedUserId",
                table: "Comments",
                column: "RelatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_RelatedUserId",
                table: "Comments",
                column: "RelatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_RelatedUserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_RelatedUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "RelatedUserId",
                table: "Comments");
        }
    }
}
