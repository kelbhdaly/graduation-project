using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalApp.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateLungRiskTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LungRiskAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Obesity = table.Column<int>(type: "int", nullable: false),
                    CoughingOfBlood = table.Column<int>(type: "int", nullable: false),
                    AlcoholUse = table.Column<int>(type: "int", nullable: false),
                    DustAllergy = table.Column<int>(type: "int", nullable: false),
                    PassiveSmoker = table.Column<int>(type: "int", nullable: false),
                    BalancedDiet = table.Column<int>(type: "int", nullable: false),
                    GeneticRisk = table.Column<int>(type: "int", nullable: false),
                    OccupationalHazards = table.Column<int>(type: "int", nullable: false),
                    ChestPain = table.Column<int>(type: "int", nullable: false),
                    AirPollution = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LungRiskAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LungRiskAnalyses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LungRiskAnalyses_UserId",
                table: "LungRiskAnalyses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LungRiskAnalyses");
        }
    }
}
