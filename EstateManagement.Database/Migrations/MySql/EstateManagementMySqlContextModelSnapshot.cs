﻿// <auto-generated />
using System;
using EstateManagement.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EstateManagement.Database.Migrations.MySql
{
    [DbContext(typeof(EstateManagementMySqlContext))]
    partial class EstateManagementMySqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EstateManagement.Database.Entities.Calendar", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("DayOfWeekNumber")
                        .HasColumnType("int");

                    b.Property<string>("DayOfWeekShort")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MonthNameLong")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MonthNameShort")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MonthNumber")
                        .HasColumnType("int");

                    b.Property<int?>("WeekNumber")
                        .HasColumnType("int");

                    b.Property<string>("WeekNumberString")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.Property<string>("YearWeekNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Date");

                    b.ToTable("calendar");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Contract", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EstateId", "OperatorId", "ContractId");

                    b.ToTable("contract");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.ContractProduct", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ProductType")
                        .HasColumnType("int");

                    b.Property<decimal?>("Value")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("EstateId", "ContractId", "ProductId");

                    b.ToTable("contractproduct");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.ContractProductTransactionFee", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TransactionFeeId")
                        .HasColumnType("char(36)");

                    b.Property<int>("CalculationType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("FeeType")
                        .HasColumnType("int");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("EstateId", "ContractId", "ProductId", "TransactionFeeId");

                    b.ToTable("contractproducttransactionfee");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Estate", b =>
                {
                    b.Property<Guid>("EstateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Reference")
                        .HasColumnType("longtext");

                    b.HasKey("EstateId");

                    b.ToTable("estate");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.EstateOperator", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("RequireCustomMerchantNumber")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("RequireCustomTerminalNumber")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("EstateId", "OperatorId");

                    b.ToTable("estateoperator");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.EstateSecurityUser", b =>
                {
                    b.Property<Guid>("SecurityUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SecurityUserId", "EstateId");

                    b.ToTable("estatesecurityuser");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.File", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FileImportLogId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FileId")
                        .HasColumnType("char(36)");

                    b.Property<string>("FileLocation")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("FileProfileId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("FileReceivedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("EstateId", "FileImportLogId", "FileId");

                    b.ToTable("file");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.FileImportLog", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FileImportLogId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ImportLogDateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("EstateId", "FileImportLogId");

                    b.ToTable("fileimportlog");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.FileImportLogFile", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FileImportLogId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FileId")
                        .HasColumnType("char(36)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("FileProfileId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("FileUploadedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<string>("OriginalFileName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("EstateId", "FileImportLogId", "FileId");

                    b.ToTable("fileimportlogfile");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.FileLine", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FileId")
                        .HasColumnType("char(36)");

                    b.Property<int>("LineNumber")
                        .HasColumnType("int");

                    b.Property<string>("FileLineData")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.HasKey("EstateId", "FileId", "LineNumber");

                    b.ToTable("fileline");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Merchant", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastStatementGenerated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Reference")
                        .HasColumnType("longtext");

                    b.Property<int>("SettlementSchedule")
                        .HasColumnType("int");

                    b.HasKey("EstateId", "MerchantId");

                    b.ToTable("merchant");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantAddress", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("char(36)");

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("longtext");

                    b.Property<string>("AddressLine3")
                        .HasColumnType("longtext");

                    b.Property<string>("AddressLine4")
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("longtext");

                    b.Property<string>("Region")
                        .HasColumnType("longtext");

                    b.Property<string>("Town")
                        .HasColumnType("longtext");

                    b.HasKey("EstateId", "MerchantId", "AddressId");

                    b.ToTable("merchantaddress");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantContact", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ContactId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.HasKey("EstateId", "MerchantId", "ContactId");

                    b.ToTable("merchantcontact");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantDevice", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeviceIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EstateId", "MerchantId", "DeviceId");

                    b.ToTable("merchantdevice");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantOperator", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("MerchantNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TerminalNumber")
                        .HasColumnType("longtext");

                    b.HasKey("EstateId", "MerchantId", "OperatorId");

                    b.ToTable("merchantoperator");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantSecurityUser", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SecurityUserId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EstateId", "MerchantId", "SecurityUserId");

                    b.ToTable("merchantsecurityuser");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantSettlementFee", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SettlementId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FeeId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("CalculatedValue")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("FeeCalculatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("FeeValue")
                        .HasColumnType("decimal(65,30)");

                    b.Property<bool>("IsSettled")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.HasKey("EstateId", "SettlementId", "TransactionId", "FeeId");

                    b.ToTable("merchantsettlementfee");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Reconciliation", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DeviceIdentifier")
                        .HasColumnType("longtext");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsAuthorised")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ResponseCode")
                        .HasColumnType("longtext");

                    b.Property<string>("ResponseMessage")
                        .HasColumnType("longtext");

                    b.Property<int>("TransactionCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("TransactionDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("TransactionTime")
                        .HasColumnType("time(6)");

                    b.Property<decimal>("TransactionValue")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("TransactionId");

                    b.ToTable("reconciliation");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.ResponseCodes", b =>
                {
                    b.Property<int>("ResponseCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ResponseCode");

                    b.ToTable("responsecodes");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Settlement", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SettlementId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("SettlementDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("EstateId", "SettlementId");

                    b.ToTable("settlement");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.StatementHeader", b =>
                {
                    b.Property<Guid>("StatementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("StatementCreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StatementGeneratedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("StatementId");

                    b.ToTable("statementheader");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.StatementLine", b =>
                {
                    b.Property<Guid>("StatementId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ActivityDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ActivityType")
                        .HasColumnType("int");

                    b.Property<string>("ActivityDescription")
                        .HasColumnType("longtext");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("InAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("OutAmount")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("StatementId", "TransactionId", "ActivityDateTime", "ActivityType");

                    b.ToTable("statementline");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Transaction", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("AuthorisationCode")
                        .HasColumnType("longtext");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("char(36)");

                    b.Property<string>("DeviceIdentifier")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAuthorised")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("OperatorIdentifier")
                        .HasColumnType("longtext");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ResponseCode")
                        .HasColumnType("longtext");

                    b.Property<string>("ResponseMessage")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("TransactionDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TransactionNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("TransactionReference")
                        .HasColumnType("longtext");

                    b.Property<int>("TransactionSource")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TransactionTime")
                        .HasColumnType("time(6)");

                    b.Property<string>("TransactionType")
                        .HasColumnType("longtext");

                    b.HasKey("EstateId", "MerchantId", "TransactionId");

                    b.ToTable("transaction");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.TransactionAdditionalRequestData", b =>
                {
                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Amount")
                        .HasColumnType("longtext");

                    b.Property<string>("CustomerAccountNumber")
                        .HasColumnType("longtext");

                    b.HasKey("EstateId", "MerchantId", "TransactionId");

                    b.ToTable("transactionadditionalrequestdata");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.TransactionAdditionalResponseData", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.HasKey("TransactionId");

                    b.ToTable("transactionadditionalresponsedata");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.TransactionFee", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FeeId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("CalculatedValue")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("CalculationType")
                        .HasColumnType("int");

                    b.Property<Guid>("EventId")
                        .HasColumnType("char(36)");

                    b.Property<int>("FeeType")
                        .HasColumnType("int");

                    b.Property<decimal>("FeeValue")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("TransactionId", "FeeId");

                    b.ToTable("transactionfee");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Voucher", b =>
                {
                    b.Property<Guid>("VoucherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("GenerateDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsGenerated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsIssued")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsRedeemed")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("IssuedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("OperatorIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RecipientEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("RecipientMobile")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RedeemedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("VoucherCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("VoucherId");

                    b.ToTable("voucher");
                });

            modelBuilder.Entity("EstateManagement.Database.ViewEntities.FileImportLogView", b =>
                {
                    b.Property<int>("FileCount")
                        .HasColumnType("int");

                    b.Property<Guid>("FileImportLogId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ImportLogDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("ImportLogDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("ImportLogTime")
                        .HasColumnType("time(6)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.ToTable("uvwFileImportLogView");

                    b.ToView("uvwFileImportLog", (string)null);
                });

            modelBuilder.Entity("EstateManagement.Database.ViewEntities.FileView", b =>
                {
                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("FailedCount")
                        .HasColumnType("int");

                    b.Property<Guid>("FileId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("FileReceivedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FileReceivedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("FileReceivedTime")
                        .HasColumnType("time(6)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("LineCount")
                        .HasColumnType("int");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<string>("MerchantName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PendingCount")
                        .HasColumnType("int");

                    b.Property<int>("SuccessCount")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.ToTable("uvwFileView");

                    b.ToView("uvwFile", (string)null);
                });

            modelBuilder.Entity("EstateManagement.Database.ViewEntities.SettlementView", b =>
                {
                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("CalculatedValue")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<string>("FeeDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsSettled")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<string>("MerchantName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MonthNumber")
                        .HasColumnType("int");

                    b.Property<string>("OperatorIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("SettlementDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("SettlementId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.Property<int>("WeekNumber")
                        .HasColumnType("int");

                    b.Property<int>("YearNumber")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("uvwSettlements", (string)null);
                });

            modelBuilder.Entity("EstateManagement.Database.ViewEntities.TransactionsView", b =>
                {
                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsAuthorised")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MonthNumber")
                        .HasColumnType("int");

                    b.Property<string>("OperatorIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ResponseCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("TransactionDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("WeekNumber")
                        .HasColumnType("int");

                    b.Property<int>("YearNumber")
                        .HasColumnType("int");

                    b.ToTable("uvwTransactionsView");

                    b.ToView("uvwTransactions", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
