using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Data.Migrations
{
    public partial class AddAdminIdToUserTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "UserTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_AdminId",
                table: "UserTasks",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_AspNetUsers_AdminId",
                table: "UserTasks",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_AspNetUsers_AdminId",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_AdminId",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "UserTasks");
        }
    }
}
