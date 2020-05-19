using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthDataAccess.Migrations
{
    public partial class UniqueKeyOnTenantName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tenants_TenantName",
                table: "Tenants",
                column: "TenantName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenants_TenantName",
                table: "Tenants");
        }
    }
}
