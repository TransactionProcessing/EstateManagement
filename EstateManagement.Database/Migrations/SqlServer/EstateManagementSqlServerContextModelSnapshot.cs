﻿// <auto-generated />
using System;
using EstateManagement.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EstateManagement.Database.Migrations.SqlServer
{
    [DbContext(typeof(EstateManagementSqlServerContext))]
    partial class EstateManagementSqlServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EstateManagement.Database.Entities.Calendar", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DayOfWeekNumber")
                        .HasColumnType("int");

                    b.Property<string>("DayOfWeekShort")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MonthNameLong")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MonthNameShort")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MonthNumber")
                        .HasColumnType("int");

                    b.Property<int?>("WeekNumber")
                        .HasColumnType("int");

                    b.Property<string>("WeekNumberString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.Property<string>("YearWeekNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Date");

                    b.ToTable("calendar");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Contract", b =>
                {
                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ContractReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContractReportingId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EstateReportingId", "OperatorId", "ContractId");

                    b.ToTable("contract");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.ContractProduct", b =>
                {
                    b.Property<int>("ContractReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ContractProductReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContractProductReportingId"));

                    b.Property<string>("DisplayText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductType")
                        .HasColumnType("int");

                    b.Property<decimal?>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ContractReportingId", "ProductId");

                    b.ToTable("contractproduct");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.ContractProductTransactionFee", b =>
                {
                    b.Property<int>("ContractProductReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("TransactionFeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CalculationType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FeeType")
                        .HasColumnType("int");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("TransactionFeeReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionFeeReportingId"));

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EstateReportingId"));

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reference")
                        .HasColumnType("nvarchar(max)");

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
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EstateOperatorReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EstateOperatorReportingId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RequireCustomMerchantNumber")
                        .HasColumnType("bit");

                    b.Property<bool>("RequireCustomTerminalNumber")
                        .HasColumnType("bit");

                    b.HasKey("EstateReportingId", "OperatorId");

                    b.ToTable("estateoperator");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.EstateSecurityUser", b =>
                {
                    b.Property<Guid>("SecurityUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SecurityUserId", "EstateReportingId");

                    b.ToTable("estatesecurityuser");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.File", b =>
                {
                    b.Property<int>("FileReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FileReportingId"));

                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FileImportLogReportingId")
                        .HasColumnType("int");

                    b.Property<string>("FileLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("FileProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FileReceivedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("FileReceivedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FileImportLogReportingId"));

                    b.Property<Guid>("FileImportLogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ImportLogDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("ImportLogDateTime")
                        .HasColumnType("datetime2");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("FileProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FileUploadedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("FileUploadedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<string>("OriginalFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.HasKey("FileReportingId", "LineNumber");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("FileReportingId", "LineNumber"));

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MerchantReportingId"));

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastSaleDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("LastSaleDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastStatementGenerated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reference")
                        .HasColumnType("nvarchar(max)");

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
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Town")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MerchantReportingId", "AddressId");

                    b.ToTable("merchantaddress");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantContact", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("ContactId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MerchantReportingId", "ContactId");

                    b.ToTable("merchantcontact");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantContract", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<int>("ContractReportingId")
                        .HasColumnType("int");

                    b.HasKey("MerchantReportingId", "ContractReportingId");

                    b.ToTable("MerchantContracts");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantDevice", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeviceIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MerchantReportingId", "DeviceId");

                    b.ToTable("merchantdevice");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantOperator", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MerchantNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TerminalNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MerchantReportingId", "OperatorId");

                    b.ToTable("merchantoperator");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.MerchantSecurityUser", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("SecurityUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("FeeCalculatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("FeeValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsSettled")
                        .HasColumnType("bit");

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionReportingId"));

                    b.Property<string>("DeviceIdentifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAuthorised")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<string>("ResponseCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TransactionCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("TransactionDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("TransactionTime")
                        .HasColumnType("time");

                    b.Property<decimal>("TransactionValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("TransactionReportingId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("TransactionReportingId"), false);

                    b.HasIndex("TransactionDate", "MerchantReportingId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("TransactionDate", "MerchantReportingId"));

                    b.HasIndex("TransactionId", "MerchantReportingId")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("TransactionId", "MerchantReportingId"), false);

                    b.ToTable("reconciliation");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.ResponseCodes", b =>
                {
                    b.Property<int>("ResponseCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ResponseCode"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ResponseCode");

                    b.ToTable("responsecodes");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Settlement", b =>
                {
                    b.Property<int>("SettlementReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SettlementReportingId"));

                    b.Property<int>("EstateReportingId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<bool>("ProcessingStarted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ProcessingStartedDateTIme")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SettlementDate")
                        .HasColumnType("date");

                    b.Property<Guid>("SettlementId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SettlementReportingId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("SettlementReportingId"), false);

                    b.HasIndex("EstateReportingId", "SettlementId")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("EstateReportingId", "SettlementId"), false);

                    b.HasIndex("SettlementDate", "EstateReportingId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("SettlementDate", "EstateReportingId"));

                    b.ToTable("settlement");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.StatementHeader", b =>
                {
                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<Guid>("StatementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StatementCreatedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("StatementCreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StatementGeneratedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("StatementGeneratedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("StatementReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatementReportingId"));

                    b.HasKey("MerchantReportingId", "StatementId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("MerchantReportingId", "StatementId"), false);

                    b.HasIndex("MerchantReportingId", "StatementGeneratedDate")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("MerchantReportingId", "StatementGeneratedDate"));

                    b.ToTable("statementheader");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.StatementLine", b =>
                {
                    b.Property<int>("StatementReportingId")
                        .HasColumnType("int");

                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ActivityDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ActivityType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ActivityDate")
                        .HasColumnType("date");

                    b.Property<string>("ActivityDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("InAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OutAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("StatementReportingId", "TransactionReportingId", "ActivityDateTime", "ActivityType");

                    b.ToTable("statementline");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.Transaction", b =>
                {
                    b.Property<int>("TransactionReportingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionReportingId"));

                    b.Property<string>("AuthorisationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContractProductReportingId")
                        .HasColumnType("int");

                    b.Property<int>("ContractReportingId")
                        .HasColumnType("int");

                    b.Property<string>("DeviceIdentifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EstateOperatorReportingId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAuthorised")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("MerchantReportingId")
                        .HasColumnType("int");

                    b.Property<string>("ResponseCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TransactionAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("TransactionDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TransactionNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TransactionSource")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TransactionTime")
                        .HasColumnType("time");

                    b.Property<string>("TransactionType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionReportingId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("TransactionReportingId"), false);

                    b.HasIndex("TransactionId")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("TransactionId"), false);

                    b.HasIndex("TransactionDate", "MerchantReportingId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("TransactionDate", "MerchantReportingId"));

                    b.HasIndex("TransactionId", "MerchantReportingId")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("TransactionId", "MerchantReportingId"), false);

                    b.ToTable("transaction");
                });

            modelBuilder.Entity("EstateManagement.Database.Entities.TransactionAdditionalRequestData", b =>
                {
                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.Property<string>("Amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerAccountNumber")
                        .HasColumnType("nvarchar(max)");

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
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("ExpiryDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("GenerateDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("GenerateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsGenerated")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIssued")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRedeemed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("IssuedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OperatorIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipientEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipientMobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RedeemedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RedeemedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransactionReportingId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("VoucherCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("VoucherId");

                    b.HasIndex("TransactionReportingId");

                    b.HasIndex("VoucherCode");

                    b.ToTable("voucher");
                });

            modelBuilder.Entity("EstateManagement.Database.ViewEntities.SettlementView", b =>
                {
                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CalculatedValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FeeDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSettled")
                        .HasColumnType("bit");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MerchantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MonthNumber")
                        .HasColumnType("int");

                    b.Property<string>("OperatorIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SettlementDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("SettlementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

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
