using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalApp.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class XrayImag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "XrayAnalysisResults");

            migrationBuilder.RenameColumn(
                name: "OutputImagePath",
                table: "XrayAnalysisResults",
                newName: "PredictedClass");

            migrationBuilder.RenameColumn(
                name: "InputImagePath",
                table: "XrayAnalysisResults",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<double>(
                name: "Confidence",
                table: "XrayAnalysisResults",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LungOpacity",
                table: "XrayAnalysisResults",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Normal",
                table: "XrayAnalysisResults",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "XrayAnalysisResults",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "ViralPneumonia",
                table: "XrayAnalysisResults",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_XrayAnalysisResults_UserId",
                table: "XrayAnalysisResults",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_XrayAnalysisResults_AspNetUsers_UserId",
                table: "XrayAnalysisResults",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XrayAnalysisResults_AspNetUsers_UserId",
                table: "XrayAnalysisResults");

            migrationBuilder.DropIndex(
                name: "IX_XrayAnalysisResults_UserId",
                table: "XrayAnalysisResults");

            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "XrayAnalysisResults");

            migrationBuilder.DropColumn(
                name: "LungOpacity",
                table: "XrayAnalysisResults");

            migrationBuilder.DropColumn(
                name: "Normal",
                table: "XrayAnalysisResults");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "XrayAnalysisResults");

            migrationBuilder.DropColumn(
                name: "ViralPneumonia",
                table: "XrayAnalysisResults");

            migrationBuilder.RenameColumn(
                name: "PredictedClass",
                table: "XrayAnalysisResults",
                newName: "OutputImagePath");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "XrayAnalysisResults",
                newName: "InputImagePath");

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "XrayAnalysisResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
