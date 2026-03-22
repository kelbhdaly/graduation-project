using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalApp.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditeAttributeglobalusingMedicalAppBusinessLogicDTOsAuthenticationDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MedicalLiences",
                table: "Doctors",
                newName: "MedicalLicense");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MedicalLicense",
                table: "Doctors",
                newName: "MedicalLiences");
        }
    }
}
