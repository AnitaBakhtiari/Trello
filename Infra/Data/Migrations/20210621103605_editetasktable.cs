using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Data.Migrations
{
    public partial class editetasktable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eaf23bef-c40b-4606-aa1a-468c0219aae3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "8699e184-5e8f-4390-b158-6a2bcd2b90c6", "fece92c4-2918-45d7-9155-93af67ff14d0", "ApplicationRole", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8699e184-5e8f-4390-b158-6a2bcd2b90c6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "eaf23bef-c40b-4606-aa1a-468c0219aae3", "7fc4dad9-26fd-45fa-86d5-ad95862464b8", "ApplicationRole", "Admin", "ADMIN" });
        }
    }
}
