using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoyageManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "voyage");

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PurchasedLicenses = table.Column<int>(type: "integer", nullable: false),
                    UsedLicenses = table.Column<int>(type: "integer", nullable: false),
                    EnrollmentSecret = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "voyage",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Workers",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HardwareId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    PasswordHash = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "voyage",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "CommandAssignments",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    WorkerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    StartedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    FinishedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandAssignments_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalSchema: "voyage",
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "GroupAssignments",
                schema: "voyage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupAssignments_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "voyage",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_GroupAssignments_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalSchema: "voyage",
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_CommandAssignments_WorkerId",
                schema: "voyage",
                table: "CommandAssignments",
                column: "WorkerId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_CommandAssignments_WorkerId_Status",
                schema: "voyage",
                table: "CommandAssignments",
                columns: new[] { "WorkerId", "Status" }
            );

            migrationBuilder.CreateIndex(
                name: "IX_GroupAssignments_GroupId_WorkerId",
                schema: "voyage",
                table: "GroupAssignments",
                columns: new[] { "GroupId", "WorkerId" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_GroupAssignments_WorkerId",
                schema: "voyage",
                table: "GroupAssignments",
                column: "WorkerId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TenantId",
                schema: "voyage",
                table: "Groups",
                column: "TenantId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Workers_TenantId",
                schema: "voyage",
                table: "Workers",
                column: "TenantId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CommandAssignments", schema: "voyage");

            migrationBuilder.DropTable(name: "GroupAssignments", schema: "voyage");

            migrationBuilder.DropTable(name: "Groups", schema: "voyage");

            migrationBuilder.DropTable(name: "Workers", schema: "voyage");

            migrationBuilder.DropTable(name: "Tenants", schema: "voyage");
        }
    }
}
