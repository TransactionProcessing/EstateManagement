using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateManagement.Database.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class task384_datamodelrefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_transactionfee",
                table: "transactionfee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactionadditionalresponsedata",
                table: "transactionadditionalresponsedata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactionadditionalrequestdata",
                table: "transactionadditionalrequestdata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transaction",
                table: "transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_statementline",
                table: "statementline");

            migrationBuilder.DropPrimaryKey(
                name: "PK_statementheader",
                table: "statementheader");

            migrationBuilder.DropPrimaryKey(
                name: "PK_settlement",
                table: "settlement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reconciliation",
                table: "reconciliation");

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
                name: "TransactionId",
                table: "voucher");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "transactionfee");

            migrationBuilder.DropColumn(
                name: "FeeId",
                table: "transactionfee");

            migrationBuilder.AddColumn<int>(
                                            name: "TransactionReportingId",
                                            table: "transactionadditionalresponsedata",
                                            type: "int",
                                            nullable: false,
                                            defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                                            name: "TransactionReportingId",
                                            table: "transactionadditionalrequestdata",
                                            type: "int",
                                            nullable: false,
                                            defaultValue: 0);

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "transactionadditionalresponsedata");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "transactionadditionalresponsedata");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "transactionadditionalresponsedata");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "transactionadditionalrequestdata");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "transactionadditionalrequestdata");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "transactionadditionalrequestdata");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "StatementId",
                table: "statementline");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "statementline");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "statementline");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "statementline");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "statementheader");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "statementheader");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "settlement");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "reconciliation");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "reconciliation");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "SettlementId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "FeeId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantsecurityuser");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "merchantsecurityuser");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantoperator");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "merchantoperator");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantdevice");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "merchantdevice");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantcontact");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "merchantcontact");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchantaddress");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "merchantaddress");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "merchant");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "fileline");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "fileline");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "fileline");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "fileimportlogfile");

            migrationBuilder.DropColumn(
                name: "FileImportLogId",
                table: "fileimportlogfile");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "fileimportlogfile");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "fileimportlogfile");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "fileimportlog");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "file");

            migrationBuilder.DropColumn(
                name: "FileImportLogId",
                table: "file");

            migrationBuilder.DropColumn(
                name: "MerchantId",
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
                name: "ContractId",
                table: "contractproducttransactionfee");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "contractproducttransactionfee");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "contractproduct");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "contractproduct");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "contract");

            migrationBuilder.AddColumn<int>(
                name: "TransactionReportingId",
                table: "voucher",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionReportingId",
                table: "transactionfee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionFeeReportingId",
                table: "transactionfee",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContractProductReportingId",
                table: "transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContractReportingId",
                table: "transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionReportingId",
                table: "transaction",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "StatementReportingId",
                table: "statementline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionReportingId",
                table: "statementline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "statementheader",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatementReportingId",
                table: "statementheader",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "settlement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SettlementReportingId",
                table: "settlement",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "reconciliation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionReportingId",
                table: "reconciliation",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "SettlementReportingId",
                table: "merchantsettlementfee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionReportingId",
                table: "merchantsettlementfee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionFeeReportingId",
                table: "merchantsettlementfee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "merchantsettlementfee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "merchantsecurityuser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "merchantoperator",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "merchantdevice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "merchantcontact",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "merchantaddress",
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
                name: "MerchantReportingId",
                table: "merchant",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FileReportingId",
                table: "fileline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionReportingId",
                table: "fileline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileImportLogReportingId",
                table: "fileimportlogfile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileReportingId",
                table: "fileimportlogfile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
                table: "fileimportlogfile",
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
                name: "FileImportLogReportingId",
                table: "fileimportlog",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "EstateReportingId",
                table: "file",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileImportLogReportingId",
                table: "file",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileReportingId",
                table: "file",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "MerchantReportingId",
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
                name: "ContractProductReportingId",
                table: "contractproducttransactionfee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionFeeReportingId",
                table: "contractproducttransactionfee",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ContractReportingId",
                table: "contractproduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContractProductReportingId",
                table: "contractproduct",
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

            migrationBuilder.AddColumn<int>(
                name: "ContractReportingId",
                table: "contract",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactionfee",
                table: "transactionfee",
                columns: new[] { "TransactionReportingId", "TransactionFeeReportingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactionadditionalresponsedata",
                table: "transactionadditionalresponsedata",
                column: "TransactionReportingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactionadditionalrequestdata",
                table: "transactionadditionalrequestdata",
                column: "TransactionReportingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transaction",
                table: "transaction",
                columns: new[] { "MerchantReportingId", "TransactionId" })
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_statementline",
                table: "statementline",
                columns: new[] { "StatementReportingId", "TransactionReportingId", "ActivityDateTime", "ActivityType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_statementheader",
                table: "statementheader",
                columns: new[] { "MerchantReportingId", "StatementId" })
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_settlement",
                table: "settlement",
                columns: new[] { "EstateReportingId", "SettlementId" })
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_reconciliation",
                table: "reconciliation",
                column: "TransactionId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantsettlementfee",
                table: "merchantsettlementfee",
                columns: new[] { "SettlementReportingId", "TransactionReportingId", "TransactionFeeReportingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantsecurityuser",
                table: "merchantsecurityuser",
                columns: new[] { "MerchantReportingId", "SecurityUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantoperator",
                table: "merchantoperator",
                columns: new[] { "MerchantReportingId", "OperatorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantdevice",
                table: "merchantdevice",
                columns: new[] { "MerchantReportingId", "DeviceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantcontact",
                table: "merchantcontact",
                columns: new[] { "MerchantReportingId", "ContactId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchantaddress",
                table: "merchantaddress",
                columns: new[] { "MerchantReportingId", "AddressId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_merchant",
                table: "merchant",
                columns: new[] { "EstateReportingId", "MerchantId" })
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileline",
                table: "fileline",
                columns: new[] { "FileReportingId", "LineNumber" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileimportlogfile",
                table: "fileimportlogfile",
                columns: new[] { "FileImportLogReportingId", "FileReportingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileimportlog",
                table: "fileimportlog",
                columns: new[] { "EstateReportingId", "FileImportLogId" })
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_file",
                table: "file",
                columns: new[] { "EstateReportingId", "FileImportLogReportingId", "FileId" })
                .Annotation("SqlServer:Clustered", false);

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
                column: "EstateId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_contractproducttransactionfee",
                table: "contractproducttransactionfee",
                columns: new[] { "ContractProductReportingId", "TransactionFeeReportingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_contractproduct",
                table: "contractproduct",
                columns: new[] { "ContractReportingId", "ContractProductReportingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_contract",
                table: "contract",
                columns: new[] { "EstateReportingId", "OperatorId", "ContractId" });

            migrationBuilder.CreateIndex(
                name: "IX_transaction_TransactionDate_MerchantReportingId",
                table: "transaction",
                columns: new[] { "TransactionDate", "MerchantReportingId" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_statementheader_MerchantReportingId_StatementGeneratedDate",
                table: "statementheader",
                columns: new[] { "MerchantReportingId", "StatementGeneratedDate" },
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_settlement_SettlementDate_EstateReportingId",
                table: "settlement",
                columns: new[] { "SettlementDate", "EstateReportingId" },
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_reconciliation_TransactionDate_MerchantReportingId",
                table: "reconciliation",
                columns: new[] { "TransactionDate", "MerchantReportingId" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_merchant_EstateReportingId_MerchantReportingId",
                table: "merchant",
                columns: new[] { "EstateReportingId", "MerchantReportingId" },
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_fileline_TransactionReportingId",
                table: "fileline",
                column: "TransactionReportingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fileimportlog_EstateReportingId_FileImportLogReportingId_ImportLogDateTime",
                table: "fileimportlog",
                columns: new[] { "EstateReportingId", "FileImportLogReportingId", "ImportLogDateTime" },
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_file_EstateReportingId_FileImportLogReportingId_FileReportingId",
                table: "file",
                columns: new[] { "EstateReportingId", "FileImportLogReportingId", "FileReportingId" },
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_estate_EstateReportingId",
                table: "estate",
                column: "EstateReportingId",
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_transactionfee",
                table: "transactionfee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactionadditionalresponsedata",
                table: "transactionadditionalresponsedata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactionadditionalrequestdata",
                table: "transactionadditionalrequestdata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transaction",
                table: "transaction");

            migrationBuilder.DropIndex(
                name: "IX_transaction_TransactionDate_MerchantReportingId",
                table: "transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_statementline",
                table: "statementline");

            migrationBuilder.DropPrimaryKey(
                name: "PK_statementheader",
                table: "statementheader");

            migrationBuilder.DropIndex(
                name: "IX_statementheader_MerchantReportingId_StatementGeneratedDate",
                table: "statementheader");

            migrationBuilder.DropPrimaryKey(
                name: "PK_settlement",
                table: "settlement");

            migrationBuilder.DropIndex(
                name: "IX_settlement_SettlementDate_EstateReportingId",
                table: "settlement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reconciliation",
                table: "reconciliation");

            migrationBuilder.DropIndex(
                name: "IX_reconciliation_TransactionDate_MerchantReportingId",
                table: "reconciliation");

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

            migrationBuilder.DropIndex(
                name: "IX_merchant_EstateReportingId_MerchantReportingId",
                table: "merchant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fileline",
                table: "fileline");

            migrationBuilder.DropIndex(
                name: "IX_fileline_TransactionReportingId",
                table: "fileline");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fileimportlogfile",
                table: "fileimportlogfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fileimportlog",
                table: "fileimportlog");

            migrationBuilder.DropIndex(
                name: "IX_fileimportlog_EstateReportingId_FileImportLogReportingId_ImportLogDateTime",
                table: "fileimportlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_file",
                table: "file");

            migrationBuilder.DropIndex(
                name: "IX_file_EstateReportingId_FileImportLogReportingId_FileReportingId",
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
                name: "IX_estate_EstateReportingId",
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
                name: "TransactionReportingId",
                table: "voucher");

            migrationBuilder.DropColumn(
                name: "TransactionReportingId",
                table: "transactionfee");

            migrationBuilder.DropColumn(
                name: "TransactionFeeReportingId",
                table: "transactionfee");

            migrationBuilder.DropColumn(
                name: "TransactionReportingId",
                table: "transactionadditionalresponsedata");

            migrationBuilder.DropColumn(
                name: "TransactionReportingId",
                table: "transactionadditionalrequestdata");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "ContractProductReportingId",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "ContractReportingId",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "TransactionReportingId",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "StatementReportingId",
                table: "statementline");

            migrationBuilder.DropColumn(
                name: "TransactionReportingId",
                table: "statementline");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "statementheader");

            migrationBuilder.DropColumn(
                name: "StatementReportingId",
                table: "statementheader");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "settlement");

            migrationBuilder.DropColumn(
                name: "SettlementReportingId",
                table: "settlement");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "reconciliation");

            migrationBuilder.DropColumn(
                name: "TransactionReportingId",
                table: "reconciliation");

            migrationBuilder.DropColumn(
                name: "SettlementReportingId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "TransactionReportingId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "TransactionFeeReportingId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "merchantsettlementfee");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "merchantsecurityuser");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "merchantoperator");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "merchantdevice");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "merchantcontact");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "merchantaddress");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "merchant");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "merchant");

            migrationBuilder.DropColumn(
                name: "FileReportingId",
                table: "fileline");

            migrationBuilder.DropColumn(
                name: "TransactionReportingId",
                table: "fileline");

            migrationBuilder.DropColumn(
                name: "FileImportLogReportingId",
                table: "fileimportlogfile");

            migrationBuilder.DropColumn(
                name: "FileReportingId",
                table: "fileimportlogfile");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
                table: "fileimportlogfile");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "fileimportlog");

            migrationBuilder.DropColumn(
                name: "FileImportLogReportingId",
                table: "fileimportlog");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "file");

            migrationBuilder.DropColumn(
                name: "FileImportLogReportingId",
                table: "file");

            migrationBuilder.DropColumn(
                name: "FileReportingId",
                table: "file");

            migrationBuilder.DropColumn(
                name: "MerchantReportingId",
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
                name: "ContractProductReportingId",
                table: "contractproducttransactionfee");

            migrationBuilder.DropColumn(
                name: "TransactionFeeReportingId",
                table: "contractproducttransactionfee");

            migrationBuilder.DropColumn(
                name: "ContractReportingId",
                table: "contractproduct");

            migrationBuilder.DropColumn(
                name: "ContractProductReportingId",
                table: "contractproduct");

            migrationBuilder.DropColumn(
                name: "EstateReportingId",
                table: "contract");

            migrationBuilder.DropColumn(
                name: "ContractReportingId",
                table: "contract");

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "voucher",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "voucher",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "transactionfee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FeeId",
                table: "transactionfee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "transactionadditionalresponsedata",
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
                name: "MerchantId",
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
                name: "MerchantId",
                table: "transactionadditionalrequestdata",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
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
                name: "MerchantId",
                table: "transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StatementId",
                table: "statementline",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "statementline",
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
                name: "MerchantId",
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
                name: "MerchantId",
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
                name: "MerchantId",
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
                name: "SettlementId",
                table: "merchantsettlementfee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "merchantsettlementfee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FeeId",
                table: "merchantsettlementfee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MerchantId",
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
                name: "MerchantId",
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
                name: "MerchantId",
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
                name: "MerchantId",
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
                name: "MerchantId",
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
                name: "MerchantId",
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
                name: "FileId",
                table: "fileline",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
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
                name: "FileImportLogId",
                table: "fileimportlogfile",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "fileimportlogfile",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MerchantId",
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
                name: "FileImportLogId",
                table: "file",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MerchantId",
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
                name: "ContractId",
                table: "contractproducttransactionfee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
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
                name: "ContractId",
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
                name: "PK_transactionfee",
                table: "transactionfee",
                columns: new[] { "TransactionId", "FeeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactionadditionalresponsedata",
                table: "transactionadditionalresponsedata",
                column: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactionadditionalrequestdata",
                table: "transactionadditionalrequestdata",
                columns: new[] { "EstateId", "MerchantId", "TransactionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_transaction",
                table: "transaction",
                columns: new[] { "EstateId", "MerchantId", "TransactionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_statementline",
                table: "statementline",
                columns: new[] { "StatementId", "TransactionId", "ActivityDateTime", "ActivityType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_statementheader",
                table: "statementheader",
                column: "StatementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_settlement",
                table: "settlement",
                columns: new[] { "EstateId", "SettlementId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_reconciliation",
                table: "reconciliation",
                column: "TransactionId");

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
