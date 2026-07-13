using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoyageManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommandAssignments_WorkerId",
                schema: "voyage",
                table: "CommandAssignments"
            );

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "voyage",
                table: "CommandAssignments",
                newName: "CommandType"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommandType",
                schema: "voyage",
                table: "CommandAssignments",
                newName: "Type"
            );

            migrationBuilder.CreateIndex(
                name: "IX_CommandAssignments_WorkerId",
                schema: "voyage",
                table: "CommandAssignments",
                column: "WorkerId",
                unique: true
            );
        }
    }
}
