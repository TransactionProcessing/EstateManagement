﻿// <auto-generated />
using System;
using EstateManagement.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EstateManagement.Database.Migrations.MySql
{
    [DbContext(typeof(EstateManagementMySqlContext))]
    [Migration("20230905184421_amountintransaction")]
    partial class amountintransaction
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
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
                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("char(36)");

                    b.Property<int>("ContractReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EstateReportingId", "OperatorId", "ContractId");

                    b.ToTable("contract");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.ContractProduct", b =>
                {
                    b.Property<int>("ContractReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<int>("ContractProductReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

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

                    b.HasKey("ContractReportingId", "ProductId");

                    b.ToTable("contractproduct");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.ContractProductTransactionFee", b =>
                {
                    b.Property<int>("ContractProductReportingId")
                        .HasColumnType("int");

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

                    b.Property<int>("TransactionFeeReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("ContractProductReportingId", "TransactionFeeId");

                    b.ToTable("contractproducttransactionfee");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Estate", b =>
                {
                    b.Property<int>("EstateReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Reference")
                        .HasColumnType("longtext");

                    b.HasKey("EstateReportingId");

                    b.HasIndex("EstateId")
                        .IsUnique();

                    b.ToTable("estate");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.EstateOperator", b =>
                {
                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("RequireCustomMerchantNumber")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("RequireCustomTerminalNumber")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("EstateReportingId", "OperatorId");

                    b.ToTable("estateoperator");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.EstateSecurityUser", b =>
                {
                    b.Property<Guid>("SecurityUserId")
                        .HasColumnType("char(36)");

                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SecurityUserId", "EstateReportingId");

                    b.ToTable("estatesecurityuser");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.File", b =>
                {
                    b.Property<int>("FileReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("FileId")
                        .HasColumnType("char(36)");

                    b.Property<int>("FileImportLogReportingId")
                        .HasColumnType("int");

                    b.Property<string>("FileLocation")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("FileProfileId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("FileReceivedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("FileReceivedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("FileReportingId");

                    b.HasIndex("FileId")
                        .IsUnique();

                    b.ToTable("file");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.FileImportLog", b =>
                {
                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<int>("FileImportLogReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<Guid>("FileImportLogId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ImportLogDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("ImportLogDateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("EstateReportingId", "FileImportLogReportingId");

                    b.HasIndex("EstateReportingId", "FileImportLogId")
                        .IsUnique();

                    b.ToTable("fileimportlog");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.FileImportLogFile", b =>
                {
                    b.Property<int>("FileImportLogReportingId")
                        .HasColumnType("int");

                    b.Property<int>("FileReportingId")
                        .HasColumnType("int");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("FileProfileId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("FileUploadedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("FileUploadedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<string>("OriginalFileName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("FileImportLogReportingId", "FileReportingId");

                    b.ToTable("fileimportlogfile");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.FileLine", b =>
                {
                    b.Property<int>("FileReportingId")
                        .HasColumnType("int");

                    b.Property<int>("LineNumber")
                        .HasColumnType("int");

                    b.Property<string>("FileLineData")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.HasKey("FileReportingId", "LineNumber")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("TransactionReportingId");

                    b.ToTable("fileline");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Merchant", b =>
                {
                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<int>("MerchantReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastSaleDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("LastSaleDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastStatementGenerated")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Reference")
                        .HasColumnType("longtext");

                    b.Property<int>("SettlementSchedule")
                        .HasColumnType("int");

                    b.HasKey("EstateReportingId", "MerchantReportingId");

                    b.HasIndex("EstateReportingId", "MerchantId")
                        .IsUnique();

                    b.ToTable("merchant");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantAddress", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

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

                    b.HasKey("MerchantReportingId", "AddressId");

                    b.ToTable("merchantaddress");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantContact", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

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

                    b.HasKey("MerchantReportingId", "ContactId");

                    b.ToTable("merchantcontact");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantDevice", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeviceIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("MerchantReportingId", "DeviceId");

                    b.ToTable("merchantdevice");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantOperator", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("MerchantNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TerminalNumber")
                        .HasColumnType("longtext");

                    b.HasKey("MerchantReportingId", "OperatorId");

                    b.ToTable("merchantoperator");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantSecurityUser", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("SecurityUserId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("MerchantReportingId", "SecurityUserId");

                    b.ToTable("merchantsecurityuser");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantSettlementFee", b =>
                {
                    b.Property<int>("SettlementReportingId")
                        .HasColumnType("int");

                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.Property<int>("TransactionFeeReportingId")
                        .HasColumnType("int");

                    b.Property<decimal>("CalculatedValue")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("FeeCalculatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("FeeValue")
                        .HasColumnType("decimal(65,30)");

                    b.Property<bool>("IsSettled")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.HasKey("SettlementReportingId", "TransactionReportingId", "TransactionFeeReportingId");

                    b.HasIndex("TransactionReportingId");

                    b.ToTable("merchantsettlementfee");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Reconciliation", b =>
                {
                    b.Property<int>("TransactionReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DeviceIdentifier")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAuthorised")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<string>("ResponseCode")
                        .HasColumnType("longtext");

                    b.Property<string>("ResponseMessage")
                        .HasColumnType("longtext");

                    b.Property<int>("TransactionCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("TransactionDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

                    b.Property<TimeSpan>("TransactionTime")
                        .HasColumnType("time(6)");

                    b.Property<decimal>("TransactionValue")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("TransactionReportingId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("TransactionDate", "MerchantReportingId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("TransactionId", "MerchantReportingId")
                        .IsUnique()
                        .HasAnnotation("SqlServer:Clustered", false);

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
                    b.Property<int>("SettlementReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<bool>("ProcessingStarted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("ProcessingStartedDateTIme")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("SettlementDate")
                        .HasColumnType("date");

                    b.Property<Guid>("SettlementId")
                        .HasColumnType("char(36)");

                    b.HasKey("SettlementReportingId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("EstateReportingId", "SettlementId")
                        .IsUnique()
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("SettlementDate", "EstateReportingId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("settlement");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.StatementHeader", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("StatementId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("StatementCreatedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("StatementCreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StatementGeneratedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("StatementGeneratedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("StatementReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("MerchantReportingId", "StatementId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("MerchantReportingId", "StatementGeneratedDate")
                        .IsUnique()
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("statementheader");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.StatementLine", b =>
                {
                    b.Property<int>("StatementReportingId")
                        .HasColumnType("int");

                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ActivityDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ActivityType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ActivityDate")
                        .HasColumnType("date");

                    b.Property<string>("ActivityDescription")
                        .HasColumnType("longtext");

                    b.Property<decimal>("InAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("OutAmount")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("StatementReportingId", "TransactionReportingId", "ActivityDateTime", "ActivityType");

                    b.ToTable("statementline");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Transaction", b =>
                {
                    b.Property<int>("TransactionReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AuthorisationCode")
                        .HasColumnType("longtext");

                    b.Property<int>("ContractProductReportingId")
                        .HasColumnType("int");

                    b.Property<int>("ContractReportingId")
                        .HasColumnType("int");

                    b.Property<string>("DeviceIdentifier")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAuthorised")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<string>("OperatorIdentifier")
                        .HasColumnType("longtext");

                    b.Property<string>("ResponseCode")
                        .HasColumnType("longtext");

                    b.Property<string>("ResponseMessage")
                        .HasColumnType("longtext");

                    b.Property<decimal>("TransactionAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("TransactionDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("char(36)");

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

                    b.HasKey("TransactionReportingId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("TransactionId")
                        .IsUnique()
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("TransactionDate", "MerchantReportingId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("TransactionId", "MerchantReportingId")
                        .IsUnique()
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("transaction");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.TransactionAdditionalRequestData", b =>
                {
                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.Property<string>("Amount")
                        .HasColumnType("longtext");

                    b.Property<string>("CustomerAccountNumber")
                        .HasColumnType("longtext");

                    b.HasKey("TransactionReportingId");

                    b.ToTable("transactionadditionalrequestdata");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.TransactionAdditionalResponseData", b =>
                {
                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.HasKey("TransactionReportingId");

                    b.ToTable("transactionadditionalresponsedata");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Voucher", b =>
                {
                    b.Property<Guid>("VoucherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("ExpiryDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("GenerateDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("GenerateDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsGenerated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsIssued")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsRedeemed")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("IssuedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("OperatorIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RecipientEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("RecipientMobile")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RedeemedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("RedeemedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("VoucherCode")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("VoucherId");

                    b.HasIndex("TransactionReportingId");

                    b.HasIndex("VoucherCode");

                    b.ToTable("voucher");
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
#pragma warning restore 612, 618
        }
    }
}
