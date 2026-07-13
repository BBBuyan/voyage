using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoyageManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssignmentTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "voyage",
                table: "CommandAssignments",
                newName: "State"
            );

            migrationBuilder.RenameIndex(
                name: "IX_CommandAssignments_WorkerId_Status",
                schema: "voyage",
                table: "CommandAssignments",
                newName: "IX_CommandAssignments_WorkerId_State"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                schema: "voyage",
                table: "CommandAssignments",
                newName: "Status"
            );

            migrationBuilder.RenameIndex(
                name: "IX_CommandAssignments_WorkerId_State",
                schema: "voyage",
                table: "CommandAssignments",
                newName: "IX_CommandAssignments_WorkerId_Status"
            );
        }
    }
}
