using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wls_backend.Migrations
{
    /// <inheritdoc />
    public partial class FixDescriptionKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DisturbanceDescription",
                table: "DisturbanceDescription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisturbanceDescription",
                table: "DisturbanceDescription",
                columns: new[] { "DisturbanceId", "Text", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DisturbanceDescription",
                table: "DisturbanceDescription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisturbanceDescription",
                table: "DisturbanceDescription",
                columns: new[] { "DisturbanceId", "Text" });
        }
    }
}
