using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalApp.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableLungRisk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "High",
                table: "LungRiskAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Low",
                table: "LungRiskAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Medium",
                table: "LungRiskAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "High",
                table: "LungRiskAnalyses");

            migrationBuilder.DropColumn(
                name: "Low",
                table: "LungRiskAnalyses");

            migrationBuilder.DropColumn(
                name: "Medium",
                table: "LungRiskAnalyses");
        }
    }
}
