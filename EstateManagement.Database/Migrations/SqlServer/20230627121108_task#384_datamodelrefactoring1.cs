using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateManagement.Database.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class task384_datamodelrefactoring1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_fileline_TransactionReportingId",
                table: "fileline");

            migrationBuilder.CreateIndex(
                name: "IX_fileline_TransactionReportingId",
                table: "fileline",
                column: "TransactionReportingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_fileline_TransactionReportingId",
                table: "fileline");

            migrationBuilder.CreateIndex(
                name: "IX_fileline_TransactionReportingId",
                table: "fileline",
                column: "TransactionReportingId",
                unique: true);
        }
    }
}
