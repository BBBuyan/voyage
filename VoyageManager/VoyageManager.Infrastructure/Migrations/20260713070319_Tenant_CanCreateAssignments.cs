using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoyageManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Tenant_CanCreateAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanCreateAssignments",
                schema: "voyage",
                table: "Tenants",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanCreateAssignments",
                schema: "voyage",
                table: "Tenants"
            );
        }
    }
}
