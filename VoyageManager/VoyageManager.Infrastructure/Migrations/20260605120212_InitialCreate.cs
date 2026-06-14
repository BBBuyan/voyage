using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoyageManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "voyage");

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PurchasedLicenses = table.Column<int>(type: "integer", nullable: false),
                    UsedLicenses = table.Column<int>(type: "integer", nullable: false),
                    EnrollmentSecret = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoyagerCommands",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CommandType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TargetType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoyagerCommands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoyagerAgents",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoyagerAgents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoyagerAgents_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "voyage",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoyagerGroups",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoyagerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoyagerGroups_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "voyage",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoyagerCommandAssignments",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VoyagerAgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoyagerCommandId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoyagerCommandAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoyagerCommandAssignments_VoyagerAgents_VoyagerAgentId",
                        column: x => x.VoyagerAgentId,
                        principalSchema: "voyage",
                        principalTable: "VoyagerAgents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoyagerCommandAssignments_VoyagerCommands_VoyagerCommandId",
                        column: x => x.VoyagerCommandId,
                        principalSchema: "voyage",
                        principalTable: "VoyagerCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoyagerGroupAssignments",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VoyagerAgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoyagerGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoyagerGroupAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoyagerGroupAssignments_VoyagerAgents_VoyagerAgentId",
                        column: x => x.VoyagerAgentId,
                        principalSchema: "voyage",
                        principalTable: "VoyagerAgents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoyagerGroupAssignments_VoyagerGroups_VoyagerGroupId",
                        column: x => x.VoyagerGroupId,
                        principalSchema: "voyage",
                        principalTable: "VoyagerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoyagerAgents_TenantId",
                schema: "voyage",
                table: "VoyagerAgents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_VoyagerCommandAssignments_VoyagerAgentId_Status",
                schema: "voyage",
                table: "VoyagerCommandAssignments",
                columns: new[] { "VoyagerAgentId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_VoyagerCommandAssignments_VoyagerAgentId_VoyagerCommandId",
                schema: "voyage",
                table: "VoyagerCommandAssignments",
                columns: new[] { "VoyagerAgentId", "VoyagerCommandId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoyagerCommandAssignments_VoyagerCommandId",
                schema: "voyage",
                table: "VoyagerCommandAssignments",
                column: "VoyagerCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_VoyagerGroupAssignments_VoyagerAgentId",
                schema: "voyage",
                table: "VoyagerGroupAssignments",
                column: "VoyagerAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_VoyagerGroupAssignments_VoyagerGroupId_VoyagerAgentId",
                schema: "voyage",
                table: "VoyagerGroupAssignments",
                columns: new[] { "VoyagerGroupId", "VoyagerAgentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoyagerGroups_TenantId",
                schema: "voyage",
                table: "VoyagerGroups",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoyagerCommandAssignments",
                schema: "voyage");

            migrationBuilder.DropTable(
                name: "VoyagerGroupAssignments",
                schema: "voyage");

            migrationBuilder.DropTable(
                name: "VoyagerCommands",
                schema: "voyage");

            migrationBuilder.DropTable(
                name: "VoyagerAgents",
                schema: "voyage");

            migrationBuilder.DropTable(
                name: "VoyagerGroups",
                schema: "voyage");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "voyage");
        }
    }
}
