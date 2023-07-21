using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateManagement.Database.Migrations.MySql
{
    /// <inheritdoc />
    public partial class addmerchanttosettlementtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "settlement",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "settlement");
        }
    }
}
