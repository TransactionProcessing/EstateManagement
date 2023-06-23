using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateManagement.Database.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class task384_addestatereportingid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_transactionadditionalrequestdata",
                table: "transactionadditionalrequestdata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transaction",
                table: "transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_settlement",
                table: "settlement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantsettlementfee",
                table: "merchantsettlementfee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantsecurityuser",
                table: "merchantsecurityuser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantoperator",
                table: "merchantoperator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantdevice",
                table: "merchantdevice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantcontact",
                table: "merchantcontact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantaddress",
                table: "merchantaddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchant",
                table: "merchant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fileline",
                table: "fileline");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fileimportlogfile",
                table: "fileimportlogfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fileimportlog",
                table: "fileimportlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_file",
                table: "file");

            migrationBuilder.DropPrimaryKey(
                name: "PK_estatesecurityuser",
                table: "estatesecurityuser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_estateoperator",
                table: "estateoperator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_estate",
                table: "estate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contractproducttransactionfee",
                table: "contractproducttransactionfee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contractproduct",
                table: "contractproduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contract",
                table: "contract");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "voucher");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "transactionadditionalresponsedata");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "transactionadditionalrequestdata");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "statementline");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "statementheader");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "settlement");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "reconciliation");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantsecurityuser");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantoperator");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantdevice");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantcontact");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantaddress");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchant");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "fileline");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "fileimportlogfile");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "fileimportlog");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "file");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "estatesecurityuser");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "estateoperator");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "contractproducttransactionfee");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "contractproduct");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "contract");

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "settlement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "merchant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "fileimportlog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "file",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "estatesecurityuser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "estateoperator",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "estate",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "contract",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactionadditionalrequestdata",
                table: "transactionadditionalrequestdata",
                columns: new[] { "MerchantId", "TransactionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_transaction",
                table: "transaction",
                columns: new[] { "MerchantId", "TransactionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_settlement",
                table: "settlement",
                columns: new[] { "EstateReportingId", "SettlementId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantsettlementfee",
                table: "merchantsettlementfee",
                columns: new[] { "SettlementId", "TransactionId", "FeeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantsecurityuser",
                table: "merchantsecurityuser",
                columns: new[] { "MerchantId", "SecurityUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantoperator",
                table: "merchantoperator",
                columns: new[] { "MerchantId", "OperatorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantdevice",
                table: "merchantdevice",
                columns: new[] { "MerchantId", "DeviceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantcontact",
                table: "merchantcontact",
                columns: new[] { "MerchantId", "ContactId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantaddress",
                table: "merchantaddress",
                columns: new[] { "MerchantId", "AddressId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchant",
                table: "merchant",
                columns: new[] { "EstateReportingId", "MerchantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileline",
                table: "fileline",
                columns: new[] { "FileId", "LineNumber" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileimportlogfile",
                table: "fileimportlogfile",
                columns: new[] { "FileImportLogId", "FileId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileimportlog",
                table: "fileimportlog",
                columns: new[] { "EstateReportingId", "FileImportLogId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_file",
                table: "file",
                columns: new[] { "EstateReportingId", "FileImportLogId", "FileId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_estatesecurityuser",
                table: "estatesecurityuser",
                columns: new[] { "SecurityUserId", "EstateReportingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_estateoperator",
                table: "estateoperator",
                columns: new[] { "EstateReportingId", "OperatorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_estate",
                table: "estate",
                column: "EstateReportingId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_contractproducttransactionfee",
                table: "contractproducttransactionfee",
                columns: new[] { "ContractId", "ProductId", "TransactionFeeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_contractproduct",
                table: "contractproduct",
                columns: new[] { "ContractId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_contract",
                table: "contract",
                columns: new[] { "EstateReportingId", "OperatorId", "ContractId" });

            migrationBuilder.CreateIndex(
                name: "IX_estate_EstateId",
                table: "estate",
                column: "EstateId",
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_transactionadditionalrequestdata",
                table: "transactionadditionalrequestdata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transaction",
                table: "transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_settlement",
                table: "settlement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantsettlementfee",
                table: "merchantsettlementfee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantsecurityuser",
                table: "merchantsecurityuser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantoperator",
                table: "merchantoperator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantdevice",
                table: "merchantdevice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantcontact",
                table: "merchantcontact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchantaddress",
                table: "merchantaddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_merchant",
                table: "merchant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fileline",
                table: "fileline");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fileimportlogfile",
                table: "fileimportlogfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fileimportlog",
                table: "fileimportlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_file",
                table: "file");

            migrationBuilder.DropPrimaryKey(
                name: "PK_estatesecurityuser",
                table: "estatesecurityuser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_estateoperator",
                table: "estateoperator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_estate",
                table: "estate");

            migrationBuilder.DropIndex(
                name: "IX_estate_EstateId",
                table: "estate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contractproducttransactionfee",
                table: "contractproducttransactionfee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contractproduct",
                table: "contractproduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contract",
                table: "contract");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "settlement");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "merchant");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "fileimportlog");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "file");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "estatesecurityuser");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "estateoperator");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "estate");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "contract");

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "voucher",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "transactionadditionalresponsedata",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "transactionadditionalrequestdata",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "statementline",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "statementheader",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "settlement",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "reconciliation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "merchantsettlementfee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "merchantsecurityuser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "merchantoperator",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "merchantdevice",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "merchantcontact",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "merchantaddress",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "merchant",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "fileline",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "fileimportlogfile",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "fileimportlog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "file",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "estatesecurityuser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "estateoperator",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "contractproducttransactionfee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "contractproduct",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "contract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactionadditionalrequestdata",
                table: "transactionadditionalrequestdata",
                columns: new[] { "EstateId", "MerchantId", "TransactionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_transaction",
                table: "transaction",
                columns: new[] { "EstateId", "MerchantId", "TransactionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_settlement",
                table: "settlement",
                columns: new[] { "EstateId", "SettlementId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantsettlementfee",
                table: "merchantsettlementfee",
                columns: new[] { "EstateId", "SettlementId", "TransactionId", "FeeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantsecurityuser",
                table: "merchantsecurityuser",
                columns: new[] { "EstateId", "MerchantId", "SecurityUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantoperator",
                table: "merchantoperator",
                columns: new[] { "EstateId", "MerchantId", "OperatorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantdevice",
                table: "merchantdevice",
                columns: new[] { "EstateId", "MerchantId", "DeviceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantcontact",
                table: "merchantcontact",
                columns: new[] { "EstateId", "MerchantId", "ContactId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantaddress",
                table: "merchantaddress",
                columns: new[] { "EstateId", "MerchantId", "AddressId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchant",
                table: "merchant",
                columns: new[] { "EstateId", "MerchantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileline",
                table: "fileline",
                columns: new[] { "EstateId", "FileId", "LineNumber" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileimportlogfile",
                table: "fileimportlogfile",
                columns: new[] { "EstateId", "FileImportLogId", "FileId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileimportlog",
                table: "fileimportlog",
                columns: new[] { "EstateId", "FileImportLogId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_file",
                table: "file",
                columns: new[] { "EstateId", "FileImportLogId", "FileId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_estatesecurityuser",
                table: "estatesecurityuser",
                columns: new[] { "SecurityUserId", "EstateId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_estateoperator",
                table: "estateoperator",
                columns: new[] { "EstateId", "OperatorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_estate",
                table: "estate",
                column: "EstateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contractproducttransactionfee",
                table: "contractproducttransactionfee",
                columns: new[] { "EstateId", "ContractId", "ProductId", "TransactionFeeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_contractproduct",
                table: "contractproduct",
                columns: new[] { "EstateId", "ContractId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_contract",
                table: "contract",
                columns: new[] { "EstateId", "OperatorId", "ContractId" });
        }
    }
}
