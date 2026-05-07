using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalApp.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class CoughEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoughAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AudioUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupportLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskScore = table.Column<double>(type: "float", nullable: false),
                    ClinicalUse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disclaimer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CovidProbability = table.Column<double>(type: "float", nullable: false),
                    NormalProbability = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoughAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoughAnalyses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoughAnalyses_UserId",
                table: "CoughAnalyses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoughAnalyses");
        }
    }
}
