using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoyageManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CommandAssignment_CancelledAt_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CancelledAt",
                schema: "voyage",
                table: "CommandAssignments",
                type: "timestamp with time zone",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAt",
                schema: "voyage",
                table: "CommandAssignments"
            );
        }
    }
}
