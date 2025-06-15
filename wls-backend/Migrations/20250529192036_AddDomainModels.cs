using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wls_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDomainModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disturbances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disturbances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DisturbanceDescriptions",
                columns: table => new
                {
                    DisturbanceId = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisturbanceDescriptions", x => new { x.DisturbanceId, x.Text });
                    table.ForeignKey(
                        name: "FK_DisturbanceDescriptions_Disturbances_DisturbanceId",
                        column: x => x.DisturbanceId,
                        principalTable: "Disturbances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisturbanceLine",
                columns: table => new
                {
                    DisturbancesId = table.Column<string>(type: "text", nullable: false),
                    LinesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisturbanceLine", x => new { x.DisturbancesId, x.LinesId });
                    table.ForeignKey(
                        name: "FK_DisturbanceLine_Disturbances_DisturbancesId",
                        column: x => x.DisturbancesId,
                        principalTable: "Disturbances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisturbanceLine_Lines_LinesId",
                        column: x => x.LinesId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisturbanceLine_LinesId",
                table: "DisturbanceLine",
                column: "LinesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisturbanceDescriptions");

            migrationBuilder.DropTable(
                name: "DisturbanceLine");

            migrationBuilder.DropTable(
                name: "Disturbances");

            migrationBuilder.DropTable(
                name: "Lines");
        }
    }
}
