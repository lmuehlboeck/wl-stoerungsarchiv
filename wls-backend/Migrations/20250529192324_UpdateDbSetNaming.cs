using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wls_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbSetNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisturbanceDescriptions_Disturbances_DisturbanceId",
                table: "DisturbanceDescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DisturbanceLine_Disturbances_DisturbancesId",
                table: "DisturbanceLine");

            migrationBuilder.DropForeignKey(
                name: "FK_DisturbanceLine_Lines_LinesId",
                table: "DisturbanceLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lines",
                table: "Lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disturbances",
                table: "Disturbances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DisturbanceDescriptions",
                table: "DisturbanceDescriptions");

            migrationBuilder.RenameTable(
                name: "Lines",
                newName: "Line");

            migrationBuilder.RenameTable(
                name: "Disturbances",
                newName: "Disturbance");

            migrationBuilder.RenameTable(
                name: "DisturbanceDescriptions",
                newName: "DisturbanceDescription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Line",
                table: "Line",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disturbance",
                table: "Disturbance",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisturbanceDescription",
                table: "DisturbanceDescription",
                columns: new[] { "DisturbanceId", "Text" });

            migrationBuilder.AddForeignKey(
                name: "FK_DisturbanceDescription_Disturbance_DisturbanceId",
                table: "DisturbanceDescription",
                column: "DisturbanceId",
                principalTable: "Disturbance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DisturbanceLine_Disturbance_DisturbancesId",
                table: "DisturbanceLine",
                column: "DisturbancesId",
                principalTable: "Disturbance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DisturbanceLine_Line_LinesId",
                table: "DisturbanceLine",
                column: "LinesId",
                principalTable: "Line",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisturbanceDescription_Disturbance_DisturbanceId",
                table: "DisturbanceDescription");

            migrationBuilder.DropForeignKey(
                name: "FK_DisturbanceLine_Disturbance_DisturbancesId",
                table: "DisturbanceLine");

            migrationBuilder.DropForeignKey(
                name: "FK_DisturbanceLine_Line_LinesId",
                table: "DisturbanceLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Line",
                table: "Line");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DisturbanceDescription",
                table: "DisturbanceDescription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disturbance",
                table: "Disturbance");

            migrationBuilder.RenameTable(
                name: "Line",
                newName: "Lines");

            migrationBuilder.RenameTable(
                name: "DisturbanceDescription",
                newName: "DisturbanceDescriptions");

            migrationBuilder.RenameTable(
                name: "Disturbance",
                newName: "Disturbances");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lines",
                table: "Lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisturbanceDescriptions",
                table: "DisturbanceDescriptions",
                columns: new[] { "DisturbanceId", "Text" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disturbances",
                table: "Disturbances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DisturbanceDescriptions_Disturbances_DisturbanceId",
                table: "DisturbanceDescriptions",
                column: "DisturbanceId",
                principalTable: "Disturbances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DisturbanceLine_Disturbances_DisturbancesId",
                table: "DisturbanceLine",
                column: "DisturbancesId",
                principalTable: "Disturbances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DisturbanceLine_Lines_LinesId",
                table: "DisturbanceLine",
                column: "LinesId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
