using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoyageManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetType",
                schema: "voyage",
                table: "VoyagerCommands");

            migrationBuilder.AddColumn<string>(
                name: "CronSchedule",
                schema: "voyage",
                table: "VoyagerCommands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HardwareId",
                schema: "voyage",
                table: "VoyagerAgents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CronSchedule",
                schema: "voyage",
                table: "VoyagerCommands");

            migrationBuilder.DropColumn(
                name: "HardwareId",
                schema: "voyage",
                table: "VoyagerAgents");

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                schema: "voyage",
                table: "VoyagerCommands",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
