using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateManagement.Database.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class merchantsettlementfee_missingindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_merchantsettlementfee_TransactionReportingId",
                table: "merchantsettlementfee",
                column: "TransactionReportingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_merchantsettlementfee_TransactionReportingId",
                table: "merchantsettlementfee");
        }
    }
}
