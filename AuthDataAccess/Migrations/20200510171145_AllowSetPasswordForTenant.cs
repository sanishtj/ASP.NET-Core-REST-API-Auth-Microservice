using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthDataAccess.Migrations
{
    public partial class AllowSetPasswordForTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowSetPassword",
                table: "Tenants",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowSetPassword",
                table: "Tenants");
        }
    }
}
