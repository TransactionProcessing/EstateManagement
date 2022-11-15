namespace EstateManagement.Repository.Tests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Contract.DomainEvents;
using Database.Contexts;
using Database.Entities;
using Estate.DomainEvents;
using FileProcessor.File.DomainEvents;
using FileProcessor.FileImportLog.DomainEvents;
using Merchant.DomainEvents;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Newtonsoft.Json;
using Shared.Exceptions;
using Shared.Logger;
using Shouldly;
using Testing;
using TransactionProcessor.Reconciliation.DomainEvents;
using TransactionProcessor.Settlement.DomainEvents;
using TransactionProcessor.Transaction.DomainEvents;
using TransactionProcessor.Voucher.DomainEvents;
using Xunit;

public class EstateReportingRepositoryTests
{
    public EstateReportingRepositoryTests()
    {
        Logger.Initialise(NullLogger.Instance);
    }

    private Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> CreateMockContextFactory()
    {
        return new Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>>();
    }

    [Fact]
    public void EstateReportingRepository_CanBeCreated_IsCreated()
    {
        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        reportingRepository.ShouldNotBeNull();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_CreateReadModel_ReadModelCreated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(),It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.NotThrow(async () => { await reportingRepository.CreateReadModel(TestData.EstateCreatedEvent, CancellationToken.None); });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddEstate_EstateAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.EstateCreatedEvent);
        EstateCreatedEvent @event = JsonConvert.DeserializeObject<EstateCreatedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddEstate(@event, CancellationToken.None);

        Estate estate = await context.Estates.SingleOrDefaultAsync(e => e.EstateId == @event.EstateId);
        estate.ShouldNotBeNull();
        estate.EstateId.ShouldBe(@event.EstateId);
        estate.Name.ShouldBe(@event.EstateName);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddEstateSecurityUser_EstateSecurityUserAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.EstateSecurityUserAddedEvent);
        SecurityUserAddedToEstateEvent @event = JsonConvert.DeserializeObject<SecurityUserAddedToEstateEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddEstateSecurityUser(@event, CancellationToken.None);

        EstateSecurityUser estateSecurityUser = await context.EstateSecurityUsers.SingleOrDefaultAsync(e => e.SecurityUserId == @event.SecurityUserId);
        estateSecurityUser.ShouldNotBeNull();
        estateSecurityUser.EstateId.ShouldBe(@event.EstateId);
        estateSecurityUser.SecurityUserId.ShouldBe(@event.SecurityUserId);
        estateSecurityUser.EmailAddress.ShouldBe(@event.EmailAddress);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddEstateOperator_EstateOperatorAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.OperatorAddedToEstateEvent);
        OperatorAddedToEstateEvent @event = JsonConvert.DeserializeObject<OperatorAddedToEstateEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddEstateOperator(@event, CancellationToken.None);

        EstateOperator estateOperator = await context.EstateOperators.SingleOrDefaultAsync(e => e.OperatorId == @event.OperatorId);
        estateOperator.ShouldNotBeNull();
        estateOperator.EstateId.ShouldBe(@event.EstateId);
        estateOperator.OperatorId.ShouldBe(@event.OperatorId);
        estateOperator.Name.ShouldBe(@event.Name);
        estateOperator.RequireCustomMerchantNumber.ShouldBe(@event.RequireCustomMerchantNumber);
        estateOperator.RequireCustomTerminalNumber.ShouldBe(@event.RequireCustomTerminalNumber);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddMerchant_MerchantAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantCreatedEvent);
        MerchantCreatedEvent @event = JsonConvert.DeserializeObject<MerchantCreatedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddMerchant(@event, CancellationToken.None);

        Merchant merchant = await context.Merchants.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.EstateId == @event.EstateId);
        merchant.ShouldNotBeNull();
        merchant.MerchantId.ShouldBe(@event.MerchantId);
        merchant.EstateId.ShouldBe(@event.EstateId);
        merchant.Name.ShouldBe(@event.MerchantName);
        merchant.CreatedDateTime.ShouldBe(@event.DateCreated);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddMerchantAddress_MerchantAddressAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        String jsonData = JsonConvert.SerializeObject(TestData.AddressAddedEvent);
        AddressAddedEvent @event = JsonConvert.DeserializeObject<AddressAddedEvent>(jsonData);

        await reportingRepository.AddMerchantAddress(@event, CancellationToken.None);

        MerchantAddress merchantAddress = await context.MerchantAddresses.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.AddressId == @event.AddressId &&
                                                                                                    e.EstateId == @event.EstateId);
        merchantAddress.ShouldNotBeNull();
        merchantAddress.MerchantId.ShouldBe(@event.MerchantId);
        merchantAddress.EstateId.ShouldBe(@event.EstateId);
        merchantAddress.AddressId.ShouldBe(@event.AddressId);
        merchantAddress.AddressLine1.ShouldBe(@event.AddressLine1);
        merchantAddress.AddressLine2.ShouldBe(@event.AddressLine2);
        merchantAddress.AddressLine3.ShouldBe(@event.AddressLine3);
        merchantAddress.AddressLine4.ShouldBe(@event.AddressLine4);
        merchantAddress.Country.ShouldBe(@event.Country);
        merchantAddress.Region.ShouldBe(@event.Region);
        merchantAddress.PostalCode.ShouldBe(@event.PostalCode);
        merchantAddress.Town.ShouldBe(@event.Town);

    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddMerchantContact_MerchantContactAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        String jsonData = JsonConvert.SerializeObject(TestData.ContactAddedEvent);
        ContactAddedEvent @event = JsonConvert.DeserializeObject<ContactAddedEvent>(jsonData);

        await reportingRepository.AddMerchantContact(@event, CancellationToken.None);

        MerchantContact merchantContact = await context.MerchantContacts.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.ContactId == @event.ContactId
                                                                                                  && e.EstateId == @event.EstateId);
        merchantContact.ShouldNotBeNull();
        merchantContact.MerchantId.ShouldBe(@event.MerchantId);
        merchantContact.ContactId.ShouldBe(@event.ContactId);
        merchantContact.EmailAddress.ShouldBe(@event.ContactEmailAddress);
        merchantContact.EstateId.ShouldBe(@event.EstateId);
        merchantContact.Name.ShouldBe(@event.ContactName);
        merchantContact.PhoneNumber.ShouldBe(@event.ContactPhoneNumber);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddMerchantDevice_MerchantDeviceAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.DeviceAddedToMerchantEvent);
        DeviceAddedToMerchantEvent @event = JsonConvert.DeserializeObject<DeviceAddedToMerchantEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddMerchantDevice(@event, CancellationToken.None);

        MerchantDevice merchantDevice = await context.MerchantDevices.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.DeviceId == @event.DeviceId && e.EstateId == @event.EstateId);
        merchantDevice.ShouldNotBeNull();
        merchantDevice.MerchantId.ShouldBe(@event.MerchantId);
        merchantDevice.EstateId.ShouldBe(@event.EstateId);
        merchantDevice.DeviceId.ShouldBe(@event.DeviceId);
        merchantDevice.DeviceIdentifier.ShouldBe(@event.DeviceIdentifier);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddMerchantSecurityUser_MerchantSecurityUserAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantSecurityUserAddedEvent);
        SecurityUserAddedToMerchantEvent @event = JsonConvert.DeserializeObject<SecurityUserAddedToMerchantEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddMerchantSecurityUser(TestData.MerchantSecurityUserAddedEvent, CancellationToken.None);

        MerchantSecurityUser merchantSecurityUser = await context.MerchantSecurityUsers.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.SecurityUserId == @event.SecurityUserId && e.EstateId == @event.EstateId);
        merchantSecurityUser.ShouldNotBeNull();
        merchantSecurityUser.MerchantId.ShouldBe(@event.MerchantId);
        merchantSecurityUser.EstateId.ShouldBe(@event.EstateId);
        merchantSecurityUser.EmailAddress.ShouldBe(@event.EmailAddress);
        merchantSecurityUser.SecurityUserId.ShouldBe(@event.SecurityUserId);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddMerchantOperator_MerchantOperatorAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.OperatorAddedToEstateEvent);
        OperatorAssignedToMerchantEvent @event = JsonConvert.DeserializeObject<OperatorAssignedToMerchantEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddMerchantOperator(@event, CancellationToken.None);

        MerchantOperator merchantOperator = await context.MerchantOperators.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.OperatorId == @event.OperatorId
                                                                                                     && e.EstateId == @event.EstateId);
        merchantOperator.ShouldNotBeNull();
        merchantOperator.MerchantId.ShouldBe(@event.MerchantId);
        merchantOperator.OperatorId.ShouldBe(@event.OperatorId);
        merchantOperator.EstateId.ShouldBe(@event.EstateId);
        merchantOperator.Name.ShouldBe(@event.Name);
        merchantOperator.MerchantNumber.ShouldBe(@event.MerchantNumber);
        merchantOperator.TerminalNumber.ShouldBe(@event.TerminalNumber);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateMerchant_MerchantSettlementScheduleUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        context.Merchants.Add(new Merchant
                              {
                                  EstateId = TestData.EstateId,
                                  MerchantId = TestData.MerchantId,
                                  SettlementSchedule = 0,
                                  CreatedDateTime = DateTime.Now,
                                  Name = TestData.MerchantName
                              });
        context.SaveChanges();
        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.SettlementScheduleChangedEvent);
        SettlementScheduleChangedEvent @event = JsonConvert.DeserializeObject<SettlementScheduleChangedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateMerchant(@event, CancellationToken.None);

        Merchant merchant = await context.Merchants.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.EstateId == @event.EstateId);
        merchant.ShouldNotBeNull();
        merchant.SettlementSchedule.ShouldBe(TestData.SettlementScheduleChangedEvent.SettlementSchedule);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateMerchant_MerchantNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.SettlementScheduleChangedEvent);
        SettlementScheduleChangedEvent @event = JsonConvert.DeserializeObject<SettlementScheduleChangedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateMerchant(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateEstate_EstateReferenceAllocated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        context.Estates.Add(new Estate
                            {
                                EstateId = TestData.EstateId,
                                CreatedDateTime = DateTime.Now,
                                Name = TestData.EstateName
                            });
        context.SaveChanges();
        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.EstateReferenceAllocatedEvent);
        EstateReferenceAllocatedEvent @event = JsonConvert.DeserializeObject<EstateReferenceAllocatedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateEstate(@event, CancellationToken.None);

        Estate estate = await context.Estates.SingleOrDefaultAsync(e => e.EstateId == @event.EstateId);
        estate.ShouldNotBeNull();
        estate.Reference.ShouldBe(TestData.EstateReferenceAllocatedEvent.EstateReference);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateEstate_EstateReferenceAllocated_EstateNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.EstateReferenceAllocatedEvent);
        EstateReferenceAllocatedEvent @event = JsonConvert.DeserializeObject<EstateReferenceAllocatedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateEstate(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateMerchant_MerchantReferenceAllocated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        context.Merchants.Add(new Merchant
                              {
                                  EstateId = TestData.EstateId,
                                  MerchantId = TestData.MerchantId,
                                  SettlementSchedule = 0,
                                  CreatedDateTime = DateTime.Now,
                                  Name = TestData.MerchantName
                              });
        context.SaveChanges();
        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantReferenceAllocatedEvent);
        MerchantReferenceAllocatedEvent @event = JsonConvert.DeserializeObject<MerchantReferenceAllocatedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateMerchant(@event, CancellationToken.None);

        Merchant merchant = await context.Merchants.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.EstateId == @event.EstateId);
        merchant.ShouldNotBeNull();
        merchant.Reference.ShouldBe(TestData.MerchantReferenceAllocatedEvent.MerchantReference);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateMerchant_MerchantReferenceAllocated_MerchantNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantReferenceAllocatedEvent);
        MerchantReferenceAllocatedEvent @event = JsonConvert.DeserializeObject<MerchantReferenceAllocatedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateMerchant(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_StartTransaction_TransactionAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionHasStartedEvent);
        TransactionHasStartedEvent @event = JsonConvert.DeserializeObject<TransactionHasStartedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.StartTransaction(@event, CancellationToken.None);

        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transaction.ShouldNotBeNull();
        transaction.EstateId.ShouldBe(@event.EstateId);
        transaction.MerchantId.ShouldBe(@event.MerchantId);
        transaction.TransactionDate.ShouldBe(@event.TransactionDateTime.Date);
        transaction.TransactionDateTime.ShouldBe(@event.TransactionDateTime);
        transaction.TransactionNumber.ShouldBe(@event.TransactionNumber);
        transaction.TransactionType.ShouldBe(@event.TransactionType);
        transaction.TransactionReference.ShouldBe(@event.TransactionReference);
        transaction.DeviceIdentifier.ShouldBe(@event.DeviceIdentifier);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_RecordTransactionAdditionalRequestData_RequestDataRecorded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.AdditionalRequestDataRecordedEvent);
        AdditionalRequestDataRecordedEvent @event = JsonConvert.DeserializeObject<AdditionalRequestDataRecordedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.RecordTransactionAdditionalRequestData(@event, CancellationToken.None);

        TransactionAdditionalRequestData transactionAdditionalRequestData = await context.TransactionsAdditionalRequestData.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transactionAdditionalRequestData.ShouldNotBeNull();
        if (@event.AdditionalTransactionRequestMetadata.TryGetValue("Amount", out String amount))
        {
            transactionAdditionalRequestData.Amount.ShouldBe(amount);
        }
        if (@event.AdditionalTransactionRequestMetadata.TryGetValue("CustomerAccountNumber", out String customerAccountNumber))
        {
            transactionAdditionalRequestData.CustomerAccountNumber.ShouldBe(customerAccountNumber);
        }

        // TODO: Compare metadata
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_RecordTransactionAdditionalResponseData_ResponseDataRecorded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.AdditionalResponseDataRecordedEvent);
        AdditionalResponseDataRecordedEvent @event = JsonConvert.DeserializeObject<AdditionalResponseDataRecordedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.RecordTransactionAdditionalResponseData(TestData.AdditionalResponseDataRecordedEvent, CancellationToken.None);

        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.MerchantId == TestData.MerchantId && e.TransactionId == TestData.TransactionId && e.EstateId == TestData.EstateId);
        transaction.ShouldNotBeNull();

        TransactionAdditionalResponseData transactionAdditionalResponseData = await context.TransactionsAdditionalResponseData.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transactionAdditionalResponseData.ShouldNotBeNull();
        transactionAdditionalResponseData.MerchantId.ShouldBe(@event.MerchantId);
        transactionAdditionalResponseData.EstateId.ShouldBe(@event.EstateId);
        transactionAdditionalResponseData.TransactionId.ShouldBe(@event.TransactionId);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateTransactionAuthorisation_LocallyAuthorised_TransactionUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionHasBeenLocallyAuthorisedEvent);
        TransactionHasBeenLocallyAuthorisedEvent @event = JsonConvert.DeserializeObject<TransactionHasBeenLocallyAuthorisedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateTransactionAuthorisation(@event, CancellationToken.None);

        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transaction.ShouldNotBeNull();
        transaction.ResponseMessage.ShouldBe(@event.ResponseMessage);
        transaction.ResponseCode.ShouldBe(@event.ResponseCode);
        transaction.IsAuthorised.ShouldBeTrue();
        transaction.AuthorisationCode.ShouldBe(@event.AuthorisationCode);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateTransactionAuthorisation_LocallyAuthorised_TransactionNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionHasBeenLocallyAuthorisedEvent);
        TransactionHasBeenLocallyAuthorisedEvent @event = JsonConvert.DeserializeObject<TransactionHasBeenLocallyAuthorisedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateTransactionAuthorisation(@event,
                                                                                                     CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateTransactionAuthorisation_LocallyDeclined_TransactionUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionHasBeenLocallyDeclinedEvent);
        TransactionHasBeenLocallyDeclinedEvent @event = JsonConvert.DeserializeObject<TransactionHasBeenLocallyDeclinedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateTransactionAuthorisation(@event, CancellationToken.None);

        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transaction.ShouldNotBeNull();
        transaction.ResponseMessage.ShouldBe(@event.ResponseMessage);
        transaction.ResponseCode.ShouldBe(@event.ResponseCode);
        transaction.IsAuthorised.ShouldBeFalse();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateTransactionAuthorisation_LocallyDeclined_TransactionNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionHasBeenLocallyDeclinedEvent);
        TransactionHasBeenLocallyDeclinedEvent @event = JsonConvert.DeserializeObject<TransactionHasBeenLocallyDeclinedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () => {
                                            await reportingRepository.UpdateTransactionAuthorisation(@event, CancellationToken.None);

                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateTransactionAuthorisation_OperatorAuthorised_TransactionUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionAuthorisedByOperatorEvent);
        TransactionAuthorisedByOperatorEvent @event = JsonConvert.DeserializeObject<TransactionAuthorisedByOperatorEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateTransactionAuthorisation(@event, CancellationToken.None);

        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transaction.ShouldNotBeNull();
        transaction.ResponseMessage.ShouldBe(@event.ResponseMessage);
        transaction.ResponseCode.ShouldBe(@event.ResponseCode);
        transaction.IsAuthorised.ShouldBeTrue();
        transaction.AuthorisationCode.ShouldBe(@event.AuthorisationCode);
        transaction.OperatorIdentifier.ShouldBe(@event.OperatorIdentifier);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateTransactionAuthorisation_OperatorAuthorised_TransactionNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionAuthorisedByOperatorEvent);
        TransactionAuthorisedByOperatorEvent @event = JsonConvert.DeserializeObject<TransactionAuthorisedByOperatorEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateTransactionAuthorisation(@event,
                                                                                                     CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateTransactionAuthorisation_OperatorDeclined_TransactionUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionDeclinedByOperatorEvent);
        TransactionDeclinedByOperatorEvent @event = JsonConvert.DeserializeObject<TransactionDeclinedByOperatorEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateTransactionAuthorisation(@event, CancellationToken.None);

        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transaction.ShouldNotBeNull();
        transaction.ResponseMessage.ShouldBe(@event.ResponseMessage);
        transaction.ResponseCode.ShouldBe(@event.ResponseCode);
        transaction.IsAuthorised.ShouldBeFalse();
        transaction.OperatorIdentifier.ShouldBe(@event.OperatorIdentifier);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateTransactionAuthorisation_OperatorDeclined_TransactionNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionDeclinedByOperatorEvent);
        TransactionDeclinedByOperatorEvent @event = JsonConvert.DeserializeObject<TransactionDeclinedByOperatorEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateTransactionAuthorisation(@event,
                                                                                                     CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_CompleteTransaction_TransactionCompleted(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionHasBeenCompletedEvent);
        TransactionHasBeenCompletedEvent @event = JsonConvert.DeserializeObject<TransactionHasBeenCompletedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.CompleteTransaction(@event, CancellationToken.None);

        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transaction.ShouldNotBeNull();
        transaction.IsCompleted.ShouldBeTrue();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_CompleteTransaction_TransactionNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionHasBeenCompletedEvent);
        TransactionHasBeenCompletedEvent @event = JsonConvert.DeserializeObject<TransactionHasBeenCompletedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () => { await reportingRepository.CompleteTransaction(@event, CancellationToken.None); });
    }


    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddContract_ContractAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ContractCreatedEvent);
        ContractCreatedEvent @event = JsonConvert.DeserializeObject<ContractCreatedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddContract(@event, CancellationToken.None);

        Contract contract = await context.Contracts.SingleOrDefaultAsync(e => e.ContractId == @event.ContractId);
        contract.ShouldNotBeNull();
        contract.OperatorId.ShouldBe(@event.OperatorId);
        contract.EstateId.ShouldBe(@event.EstateId);
        contract.ContractId.ShouldBe(@event.ContractId);
        contract.Description.ShouldBe(@event.Description);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddContractProduct_FixedValue_ContractProductAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FixedValueProductAddedToContractEvent);
        FixedValueProductAddedToContractEvent @event = JsonConvert.DeserializeObject<FixedValueProductAddedToContractEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddContractProduct(@event, CancellationToken.None);

        ContractProduct contractProduct = await context.ContractProducts.SingleOrDefaultAsync(e => e.ContractId == @event.ContractId &&
                                                                                                   e.ProductId == @event.ProductId);
        contractProduct.ShouldNotBeNull();
        contractProduct.Value.ShouldBe(@event.Value);
        contractProduct.EstateId.ShouldBe(@event.EstateId);
        contractProduct.ContractId.ShouldBe(@event.ContractId);
        contractProduct.DisplayText.ShouldBe(@event.DisplayText);
        contractProduct.ProductId.ShouldBe(@event.ProductId);
        contractProduct.ProductName.ShouldBe(@event.ProductName);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddContractProduct_VariableValue_ContractProductAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.VariableValueProductAddedToContractEvent);
        VariableValueProductAddedToContractEvent @event = JsonConvert.DeserializeObject<VariableValueProductAddedToContractEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddContractProduct(@event, CancellationToken.None);

        ContractProduct contractProduct = await context.ContractProducts.SingleOrDefaultAsync(e => e.ContractId == @event.ContractId &&
                                                                                                   e.ProductId == @event.ProductId);
        contractProduct.ShouldNotBeNull();
        contractProduct.Value.ShouldBeNull();
        contractProduct.EstateId.ShouldBe(@event.EstateId);
        contractProduct.ContractId.ShouldBe(@event.ContractId);
        contractProduct.DisplayText.ShouldBe(@event.DisplayText);
        contractProduct.ProductId.ShouldBe(@event.ProductId);
        contractProduct.ProductName.ShouldBe(@event.ProductName);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddContractProductTransactionFee_ContractProductTransactionFee(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionFeeForProductAddedToContractEvent);
        TransactionFeeForProductAddedToContractEvent @event = JsonConvert.DeserializeObject<TransactionFeeForProductAddedToContractEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddContractProductTransactionFee(@event, CancellationToken.None);

        ContractProductTransactionFee contractProductTransactionFee = await context.ContractProductTransactionFees.SingleOrDefaultAsync(e => e.ContractId == @event.ContractId &&
            e.ProductId == @event.ProductId &&
            e.TransactionFeeId == @event.TransactionFeeId);
        contractProductTransactionFee.ShouldNotBeNull();
        contractProductTransactionFee.EstateId.ShouldBe(@event.EstateId);
        contractProductTransactionFee.ContractId.ShouldBe(@event.ContractId);
        contractProductTransactionFee.ProductId.ShouldBe(@event.ProductId);
        contractProductTransactionFee.Description.ShouldBe(@event.Description);
        contractProductTransactionFee.CalculationType.ShouldBe(@event.CalculationType);
        contractProductTransactionFee.FeeType.ShouldBe(@event.FeeType);
        contractProductTransactionFee.IsEnabled.ShouldBeTrue();
        contractProductTransactionFee.Value.ShouldBe(@event.Value);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddProductDetailsToTransaction_ProductDetailsAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ProductDetailsAddedToTransactionEvent);
        ProductDetailsAddedToTransactionEvent @event = JsonConvert.DeserializeObject<ProductDetailsAddedToTransactionEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddProductDetailsToTransaction(@event, CancellationToken.None);

        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transaction.ShouldNotBeNull();
        transaction.ContractId.ShouldBe(@event.ContractId);
        transaction.ProductId.ShouldBe(@event.ProductId);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddProductDetailsToTransaction_TransactionNotFound_ErrorThroen(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ProductDetailsAddedToTransactionEvent);
        ProductDetailsAddedToTransactionEvent @event = JsonConvert.DeserializeObject<ProductDetailsAddedToTransactionEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);
        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.AddProductDetailsToTransaction(@event,
                                                                                                     CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddSourceDetailsToTransaction_TransactionSourceAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionSourceAddedToTransactionEvent);
        TransactionSourceAddedToTransactionEvent @event = JsonConvert.DeserializeObject<TransactionSourceAddedToTransactionEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddSourceDetailsToTransaction(@event, CancellationToken.None);

        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        transaction.ShouldNotBeNull();
        transaction.TransactionSource.ShouldBe(@event.TransactionSource);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddSourceDetailsToTransaction_TransactionNotFound_ErrorThroen(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionSourceAddedToTransactionEvent);
        TransactionSourceAddedToTransactionEvent @event = JsonConvert.DeserializeObject<TransactionSourceAddedToTransactionEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);
        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.AddSourceDetailsToTransaction(@event,
                                                                                                    CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFeeDetailsToTransaction_MerchantFeeAddedToTransactionEvent_FeeAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId,
                                                AuthorisationCode = TestData.AuthorisationCode,
                                                DeviceIdentifier = TestData.DeviceIdentifier,
                                                OperatorIdentifier = TestData.OperatorIdentifier,
                                                
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantFeeAddedToTransactionEvent);
        MerchantFeeAddedToTransactionEnrichedEvent @event = JsonConvert.DeserializeObject<MerchantFeeAddedToTransactionEnrichedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddFeeDetailsToTransaction(@event, CancellationToken.None);

        TransactionFee transactionFee = await context.TransactionFees.SingleOrDefaultAsync(e => e.TransactionId == @event.TransactionId && e.FeeId == @event.FeeId);
        transactionFee.ShouldNotBeNull();
        transactionFee.FeeId.ShouldBe(@event.FeeId);
        transactionFee.CalculatedValue.ShouldBe(@event.CalculatedValue);
        transactionFee.CalculationType.ShouldBe(@event.FeeCalculationType);
        transactionFee.EventId.ShouldBe(@event.EventId);
        transactionFee.FeeType.ShouldBe(0);
        transactionFee.FeeValue.ShouldBe(@event.FeeValue);
        transactionFee.TransactionId.ShouldBe(@event.TransactionId);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFeeDetailsToTransaction_MerchantFeeAddedToTransactionEvent_TransactionNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantFeeAddedToTransactionEvent);
        MerchantFeeAddedToTransactionEnrichedEvent @event = JsonConvert.DeserializeObject<MerchantFeeAddedToTransactionEnrichedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);
        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.AddFeeDetailsToTransaction(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFeeDetailsToTransaction_ServiceProviderFeeAddedToTransactionEvent_FeeAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Transactions.AddAsync(new Transaction
                                            {
                                                TransactionId = TestData.TransactionId,
                                                MerchantId = TestData.MerchantId,
                                                EstateId = TestData.EstateId
                                            });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ServiceProviderFeeAddedToTransactionEvent);
        ServiceProviderFeeAddedToTransactionEnrichedEvent @event = JsonConvert.DeserializeObject<ServiceProviderFeeAddedToTransactionEnrichedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddFeeDetailsToTransaction(@event, CancellationToken.None);

        TransactionFee transactionFee = await context.TransactionFees.SingleOrDefaultAsync(e => e.TransactionId == @event.TransactionId && e.FeeId == @event.FeeId);
        transactionFee.ShouldNotBeNull();
        transactionFee.FeeId.ShouldBe(@event.FeeId);
        transactionFee.CalculatedValue.ShouldBe(@event.CalculatedValue);
        transactionFee.CalculationType.ShouldBe(@event.FeeCalculationType);
        transactionFee.FeeType.ShouldBe(1);
        transactionFee.FeeValue.ShouldBe(@event.FeeValue);
        transactionFee.TransactionId.ShouldBe(@event.TransactionId);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFeeDetailsToTransaction_ServiceProviderFeeAddedToTransactionEvent_TransactionNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ServiceProviderFeeAddedToTransactionEvent);
        ServiceProviderFeeAddedToTransactionEnrichedEvent @event = JsonConvert.DeserializeObject<ServiceProviderFeeAddedToTransactionEnrichedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);
        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.AddFeeDetailsToTransaction(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_DisableContractProductTransactionFee_TransactionFeeDisabled(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.ContractProductTransactionFees.AddAsync(new ContractProductTransactionFee
                                                              {
                                                                  EstateId = TestData.EstateId,
                                                                  ProductId = TestData.ProductId,
                                                                  TransactionFeeId = TestData.TransactionFeeId,
                                                                  Value = TestData.FeeValue,
                                                                  FeeType = TestData.FeeType,
                                                                  Description = TestData.TransactionFeeDescription,
                                                                  CalculationType = TestData.FeeCalculationType,
                                                                  IsEnabled = true,
                                                                  ContractId = TestData.ContractId
                                                              });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionFeeForProductDisabledEvent);
        TransactionFeeForProductDisabledEvent @event = JsonConvert.DeserializeObject<TransactionFeeForProductDisabledEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.DisableContractProductTransactionFee(@event, CancellationToken.None);

        ContractProductTransactionFee transactionFee = await context.ContractProductTransactionFees.SingleAsync(t => t.TransactionFeeId == @event.TransactionFeeId);
        transactionFee.ShouldNotBeNull();
        transactionFee.IsEnabled.ShouldBeFalse();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_DisableContractProductTransactionFee_TransactionFeeNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.TransactionFeeForProductDisabledEvent);
        TransactionFeeForProductDisabledEvent @event = JsonConvert.DeserializeObject<TransactionFeeForProductDisabledEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.DisableContractProductTransactionFee(@event,
                                                                                                           CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_StartReconciliation_ReconciliationAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ReconciliationHasStartedEvent);
        ReconciliationHasStartedEvent @event = JsonConvert.DeserializeObject<ReconciliationHasStartedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.StartReconciliation(@event, CancellationToken.None);

        Reconciliation reconciliation = await context.Reconciliations.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        reconciliation.ShouldNotBeNull();
        reconciliation.TransactionId.ShouldBe(@event.TransactionId);
        reconciliation.EstateId.ShouldBe(@event.EstateId);
        reconciliation.TransactionDateTime.ShouldBe(@event.TransactionDateTime);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateReconciliationOverallTotals_ReconciliationUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Reconciliations.AddAsync(new Reconciliation
                                               {
                                                   TransactionId = TestData.TransactionId,
                                                   MerchantId = TestData.MerchantId,
                                                   EstateId = TestData.EstateId,
                                                   TransactionDate = TestData.TransactionDateTime.Date,
                                                   TransactionDateTime = TestData.TransactionDateTime,
                                                   TransactionTime = TestData.TransactionDateTime.TimeOfDay
                                               });
        await context.SaveChangesAsync();

        String jsonData = JsonConvert.SerializeObject(TestData.OverallTotalsRecordedEvent);
        OverallTotalsRecordedEvent @event = JsonConvert.DeserializeObject<OverallTotalsRecordedEvent>(jsonData);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateReconciliationOverallTotals(@event, CancellationToken.None);

        Reconciliation reconciliation = await context.Reconciliations.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        reconciliation.TransactionCount.ShouldBe(@event.TransactionCount);
        reconciliation.TransactionValue.ShouldBe(@event.TransactionValue);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateReconciliationOverallTotals_ReconciliationNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.OverallTotalsRecordedEvent);
        OverallTotalsRecordedEvent @event = JsonConvert.DeserializeObject<OverallTotalsRecordedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateReconciliationOverallTotals(@event,
                                                                                                        CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateReconciliationStatus_Authorised_ReconciliationUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Reconciliations.AddAsync(new Reconciliation
                                               {
                                                   TransactionId = TestData.TransactionId,
                                                   MerchantId = TestData.MerchantId,
                                                   EstateId = TestData.EstateId,
                                                   TransactionDate = TestData.TransactionDateTime.Date,
                                                   TransactionDateTime = TestData.TransactionDateTime,
                                                   TransactionTime = TestData.TransactionDateTime.TimeOfDay
                                               });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ReconciliationHasBeenLocallyAuthorisedEvent);
        ReconciliationHasBeenLocallyAuthorisedEvent @event = JsonConvert.DeserializeObject<ReconciliationHasBeenLocallyAuthorisedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateReconciliationStatus(@event, CancellationToken.None);

        Reconciliation reconciliation = await context.Reconciliations.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        reconciliation.ShouldNotBeNull();
        reconciliation.IsAuthorised.ShouldBeTrue();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateReconciliationStatus_Authorised_ReconciliationNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ReconciliationHasBeenLocallyAuthorisedEvent);
        ReconciliationHasBeenLocallyAuthorisedEvent @event = JsonConvert.DeserializeObject<ReconciliationHasBeenLocallyAuthorisedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateReconciliationStatus(@event,
                                                                                                 CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateReconciliationStatus_Declined_ReconciliationUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Reconciliations.AddAsync(new Reconciliation
                                               {
                                                   TransactionId = TestData.TransactionId,
                                                   MerchantId = TestData.MerchantId,
                                                   EstateId = TestData.EstateId,
                                                   TransactionDate = TestData.TransactionDateTime.Date,
                                                   TransactionDateTime = TestData.TransactionDateTime,
                                                   TransactionTime = TestData.TransactionDateTime.TimeOfDay
                                               });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ReconciliationHasBeenLocallyDeclinedEvent);
        ReconciliationHasBeenLocallyDeclinedEvent @event = JsonConvert.DeserializeObject<ReconciliationHasBeenLocallyDeclinedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateReconciliationStatus(@event, CancellationToken.None);

        Reconciliation reconciliation = await context.Reconciliations.SingleOrDefaultAsync(e => e.MerchantId == TestData.MerchantId && e.TransactionId == TestData.TransactionId && e.EstateId == TestData.EstateId);
        reconciliation.ShouldNotBeNull();
        reconciliation.IsAuthorised.ShouldBeFalse();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateReconciliationStatus_Declined_ReconciliationNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ReconciliationHasBeenLocallyDeclinedEvent);
        ReconciliationHasBeenLocallyDeclinedEvent @event = JsonConvert.DeserializeObject<ReconciliationHasBeenLocallyDeclinedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateReconciliationStatus(@event,
                                                                                                 CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_CompleteReconciliation_ReconciliationUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Reconciliations.AddAsync(new Reconciliation
                                               {
                                                   TransactionId = TestData.TransactionId,
                                                   MerchantId = TestData.MerchantId,
                                                   EstateId = TestData.EstateId,
                                                   TransactionDate = TestData.TransactionDateTime.Date,
                                                   TransactionDateTime = TestData.TransactionDateTime,
                                                   TransactionTime = TestData.TransactionDateTime.TimeOfDay
                                               });
        await context.SaveChangesAsync();

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ReconciliationHasCompletedEvent);
        ReconciliationHasCompletedEvent @event = JsonConvert.DeserializeObject<ReconciliationHasCompletedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.CompleteReconciliation(@event, CancellationToken.None);

        Reconciliation reconciliation = await context.Reconciliations.SingleOrDefaultAsync(e => e.MerchantId == @event.MerchantId && e.TransactionId == @event.TransactionId && e.EstateId == @event.EstateId);
        reconciliation.ShouldNotBeNull();
        reconciliation.IsCompleted.ShouldBeTrue();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_CompleteReconciliation_ReconciliationNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ReconciliationHasCompletedEvent);
        ReconciliationHasCompletedEvent @event = JsonConvert.DeserializeObject<ReconciliationHasCompletedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.CompleteReconciliation(@event,
                                                                                             CancellationToken.None);
                                        });
    }
    
    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddGeneratedVoucher_VoucherAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.VoucherGeneratedEvent);
        VoucherGeneratedEvent @event = JsonConvert.DeserializeObject<VoucherGeneratedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddGeneratedVoucher(@event, CancellationToken.None);

        Voucher voucher = await context.Vouchers.SingleOrDefaultAsync(e => e.VoucherId == @event.VoucherId && e.EstateId == @event.EstateId);
        voucher.ShouldNotBeNull();
        voucher.EstateId.ShouldBe(@event.EstateId);
        voucher.TransactionId.ShouldBe(@event.TransactionId);
        voucher.VoucherId.ShouldBe(@event.VoucherId);
        voucher.GenerateDateTime.ShouldBe(@event.GeneratedDateTime);
        voucher.OperatorIdentifier.ShouldBe(@event.OperatorIdentifier);
        voucher.Value.ShouldBe(@event.Value);
        voucher.VoucherCode.ShouldBe(@event.VoucherCode);
        voucher.ExpiryDate.ShouldBe(@event.ExpiryDateTime.Date);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateVoucherIssueDetails_VoucherUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Vouchers.AddAsync(new Voucher
                                        {
                                            EstateId = TestData.EstateId,
                                            VoucherId = TestData.VoucherId
                                        }, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.VoucherIssuedEvent);
        VoucherIssuedEvent @event = JsonConvert.DeserializeObject<VoucherIssuedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateVoucherIssueDetails(@event, CancellationToken.None);

        Voucher voucher = await context.Vouchers.SingleOrDefaultAsync(e => e.VoucherId == @event.VoucherId && e.EstateId == @event.EstateId);
        voucher.IsIssued.ShouldBeTrue();
        voucher.RecipientEmail.ShouldBe(@event.RecipientEmail);
        voucher.RecipientMobile.ShouldBe(@event.RecipientMobile);
        voucher.IssuedDateTime.ShouldBe(@event.IssuedDateTime);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateVoucherIssueDetails_VoucherNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.VoucherIssuedEvent);
        VoucherIssuedEvent @event = JsonConvert.DeserializeObject<VoucherIssuedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateVoucherIssueDetails(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateVoucherRedemptionDetails_VoucherUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Vouchers.AddAsync(new Voucher
                                        {
                                            EstateId = TestData.EstateId,
                                            VoucherId = TestData.VoucherId
                                        }, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.VoucherFullyRedeemedEvent);
        VoucherFullyRedeemedEvent @event = JsonConvert.DeserializeObject<VoucherFullyRedeemedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateVoucherRedemptionDetails(@event, CancellationToken.None);

        Voucher voucher = await context.Vouchers.SingleOrDefaultAsync(e => e.VoucherId == TestData.VoucherId && e.EstateId == TestData.EstateId);
        voucher.IsRedeemed.ShouldBeTrue();
        voucher.RedeemedDateTime.ShouldBe(@event.RedeemedDateTime);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateVoucherRedemptionDetails_VoucherNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.VoucherFullyRedeemedEvent);
        VoucherFullyRedeemedEvent @event = JsonConvert.DeserializeObject<VoucherFullyRedeemedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateVoucherRedemptionDetails(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFileImportLog_ImportLogAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.ImportLogCreatedEvent);
        ImportLogCreatedEvent @event = JsonConvert.DeserializeObject<ImportLogCreatedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddFileImportLog(@event, CancellationToken.None);

        FileImportLog fileImportLog = await context.FileImportLogs.SingleOrDefaultAsync(e => e.FileImportLogId == @event.FileImportLogId);
        fileImportLog.ShouldNotBeNull();
        fileImportLog.EstateId.ShouldBe(@event.EstateId);
        fileImportLog.FileImportLogId.ShouldBe(@event.FileImportLogId);
        fileImportLog.ImportLogDateTime.ShouldBe(@event.ImportLogDateTime);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFileToImportLog_FileAddedToImportLog(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.FileImportLogs.AddAsync(new FileImportLog
                                              {
                                                  FileImportLogId = TestData.FileAddedToImportLogEvent.FileImportLogId,
                                                  EstateId = TestData.FileAddedToImportLogEvent.EstateId
                                              }, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileAddedToImportLogEvent);
        FileAddedToImportLogEvent @event = JsonConvert.DeserializeObject<FileAddedToImportLogEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddFileToImportLog(@event, CancellationToken.None);

        FileImportLogFile fileImportLogFile = await context.FileImportLogFiles.SingleOrDefaultAsync(e => e.FileImportLogId == @event.FileImportLogId);
        fileImportLogFile.ShouldNotBeNull();
        fileImportLogFile.FileImportLogId.ShouldBe(@event.FileImportLogId);
        fileImportLogFile.MerchantId.ShouldBe(@event.MerchantId);
        fileImportLogFile.FileId.ShouldBe(@event.FileId);
        fileImportLogFile.FilePath.ShouldBe(@event.FilePath);
        fileImportLogFile.EstateId.ShouldBe(@event.EstateId);
        fileImportLogFile.FileProfileId.ShouldBe(@event.FileProfileId);
        fileImportLogFile.FileUploadedDateTime.ShouldBe(@event.FileUploadedDateTime);
        fileImportLogFile.OriginalFileName.ShouldBe(@event.OriginalFileName);

    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFileToImportLog_ImportLogNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileAddedToImportLogEvent);
        FileAddedToImportLogEvent @event = JsonConvert.DeserializeObject<FileAddedToImportLogEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.AddFileToImportLog(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFile_FileAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileCreatedEvent);
        FileCreatedEvent @event = JsonConvert.DeserializeObject<FileCreatedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddFile(@event, CancellationToken.None);

        File file = await context.Files.SingleOrDefaultAsync(e => e.FileId == @event.FileId);
        file.ShouldNotBeNull();
        file.EstateId.ShouldBe(@event.EstateId);
        file.FileImportLogId.ShouldBe(@event.FileImportLogId);
        file.IsCompleted.ShouldBeFalse();
        file.FileLocation.ShouldBe(@event.FileLocation);
        file.FileProfileId.ShouldBe(@event.FileProfileId);
        file.FileId.ShouldBe(@event.FileId);
        file.FileReceivedDateTime.ShouldBe(@event.FileReceivedDateTime);
        file.MerchantId.ShouldBe(@event.MerchantId);
        file.UserId.ShouldBe(@event.UserId);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFileLineToFile_FileLineAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Files.AddAsync(new File
                                     {
                                         FileId = TestData.FileLineAddedEvent.FileId
                                     }, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileLineAddedEvent);
        FileLineAddedEvent @event = JsonConvert.DeserializeObject<FileLineAddedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddFileLineToFile(@event, CancellationToken.None);

        FileLine fileLine = await context.FileLines.SingleOrDefaultAsync(e => e.FileId == @event.FileId && e.LineNumber == @event.LineNumber);
        fileLine.ShouldNotBeNull();
        fileLine.EstateId.ShouldBe(@event.EstateId);
        fileLine.LineNumber.ShouldBe(@event.LineNumber);
        fileLine.Status.ShouldBe("P");
        fileLine.FileId.ShouldBe(@event.FileId);
        fileLine.FileLineData.ShouldBe(@event.FileLine);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddFileLineToFile_FileNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileLineAddedEvent);
        FileLineAddedEvent @event = JsonConvert.DeserializeObject<FileLineAddedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.AddFileLineToFile(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateFileLine_Successful_FileLineAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Files.AddAsync(new File
                                     {
                                         FileId = TestData.FileLineProcessingSuccessfulEvent.FileId
                                     }, CancellationToken.None);
        await context.FileLines.AddAsync(new FileLine
                                         {
                                             FileId = TestData.FileLineProcessingSuccessfulEvent.FileId,
                                             LineNumber = TestData.FileLineProcessingSuccessfulEvent.LineNumber
                                         });
        await context.SaveChangesAsync(CancellationToken.None);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileLineProcessingSuccessfulEvent);
        FileLineProcessingSuccessfulEvent @event = JsonConvert.DeserializeObject<FileLineProcessingSuccessfulEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateFileLine(@event, CancellationToken.None);

        FileLine fileLine = await context.FileLines.SingleOrDefaultAsync(e => e.FileId == @event.FileId && e.LineNumber == @event.LineNumber);
        fileLine.ShouldNotBeNull();
        fileLine.Status.ShouldBe("S");
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateFileLine_Successful_LineNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Files.AddAsync(new File
                                     {
                                         FileId = TestData.FileLineProcessingSuccessfulEvent.FileId
                                     }, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileLineProcessingSuccessfulEvent);
        FileLineProcessingSuccessfulEvent @event = JsonConvert.DeserializeObject<FileLineProcessingSuccessfulEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateFileLine(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateFileLine_Failed_FileLineAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Files.AddAsync(new File
                                     {
                                         FileId = TestData.FileLineProcessingFailedEvent.FileId
                                     }, CancellationToken.None);
        await context.FileLines.AddAsync(new FileLine
                                         {
                                             FileId = TestData.FileLineProcessingFailedEvent.FileId,
                                             LineNumber = TestData.FileLineProcessingFailedEvent.LineNumber
                                         });
        await context.SaveChangesAsync(CancellationToken.None);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileLineProcessingFailedEvent);
        FileLineProcessingFailedEvent @event = JsonConvert.DeserializeObject<FileLineProcessingFailedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateFileLine(@event, CancellationToken.None);

        FileLine fileLine = await context.FileLines.SingleOrDefaultAsync(e => e.FileId == @event.FileId && e.LineNumber == @event.LineNumber);
        fileLine.ShouldNotBeNull();
        fileLine.Status.ShouldBe("F");
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateFileLine_Failed_LineNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Files.AddAsync(new File
                                     {
                                         FileId = TestData.FileLineProcessingFailedEvent.FileId
                                     }, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileLineProcessingFailedEvent);
        FileLineProcessingFailedEvent @event = JsonConvert.DeserializeObject<FileLineProcessingFailedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateFileLine(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateFileLine_Ignored_FileLineAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Files.AddAsync(new File
                                     {
                                         FileId = TestData.FileLineProcessingIgnoredEvent.FileId
                                     }, CancellationToken.None);
        await context.FileLines.AddAsync(new FileLine
                                         {
                                             FileId = TestData.FileLineProcessingIgnoredEvent.FileId,
                                             LineNumber = TestData.FileLineProcessingIgnoredEvent.LineNumber
                                         });
        await context.SaveChangesAsync(CancellationToken.None);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileLineProcessingIgnoredEvent);
        FileLineProcessingIgnoredEvent @event = JsonConvert.DeserializeObject<FileLineProcessingIgnoredEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateFileLine(@event, CancellationToken.None);

        FileLine fileLine = await context.FileLines.SingleOrDefaultAsync(e => e.FileId == @event.FileId && e.LineNumber == @event.LineNumber);
        fileLine.ShouldNotBeNull();
        fileLine.Status.ShouldBe("I");
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateFileLine_Ignored_LineNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Files.AddAsync(new File
                                     {
                                         FileId = TestData.FileLineProcessingIgnoredEvent.FileId
                                     }, CancellationToken.None);

        await context.SaveChangesAsync(CancellationToken.None);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileLineProcessingIgnoredEvent);
        FileLineProcessingIgnoredEvent @event = JsonConvert.DeserializeObject<FileLineProcessingIgnoredEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateFileLine(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateFileAsCompleted_FileUpdated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        await context.Files.AddAsync(new File
                                     {
                                         FileId = TestData.FileProcessingCompletedEvent.FileId
                                     }, CancellationToken.None);

        await context.SaveChangesAsync(CancellationToken.None);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileProcessingCompletedEvent);
        FileProcessingCompletedEvent @event = JsonConvert.DeserializeObject<FileProcessingCompletedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.UpdateFileAsComplete(@event, CancellationToken.None);

        File file = await context.Files.SingleOrDefaultAsync(e => e.FileId == @event.FileId);
        file.ShouldNotBeNull();
        file.IsCompleted.ShouldBeTrue();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_UpdateFileAsCompleted_FileNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.FileProcessingCompletedEvent);
        FileProcessingCompletedEvent @event = JsonConvert.DeserializeObject<FileProcessingCompletedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.UpdateFileAsComplete(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_CreateSettlement_SettlementCreated(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.SettlementCreatedForDateEvent);
        SettlementCreatedForDateEvent @event = JsonConvert.DeserializeObject<SettlementCreatedForDateEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.CreateSettlement(@event, CancellationToken.None);

        Settlement settlement = await context.Settlements.SingleOrDefaultAsync(s => s.SettlementId == @event.SettlementId);
        settlement.ShouldNotBeNull();
        settlement.EstateId.ShouldBe(@event.EstateId);
        settlement.SettlementId.ShouldBe(@event.SettlementId);
        settlement.SettlementDate.ShouldBe(@event.SettlementDate);
        settlement.IsCompleted.ShouldBeFalse();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_AddPendingMerchantFeeToSettlement_PendingMerchantFeeAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantFeeAddedPendingSettlementEvent);
        MerchantFeeAddedPendingSettlementEvent @event = JsonConvert.DeserializeObject<MerchantFeeAddedPendingSettlementEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddPendingMerchantFeeToSettlement(@event, CancellationToken.None);

        MerchantSettlementFee merchantSettlementFee = await context.MerchantSettlementFees.SingleOrDefaultAsync(s => s.SettlementId == @event.SettlementId &&
                                                                                                                    s.EstateId == @event.EstateId &&
                                                                                                                    s.MerchantId == @event.MerchantId &&
                                                                                                                    s.FeeId == @event.FeeId);
        merchantSettlementFee.ShouldNotBeNull();
        merchantSettlementFee.EstateId.ShouldBe(@event.EstateId);
        merchantSettlementFee.SettlementId.ShouldBe(@event.SettlementId);
        merchantSettlementFee.FeeId.ShouldBe(@event.FeeId);
        merchantSettlementFee.IsSettled.ShouldBeFalse();
        merchantSettlementFee.MerchantId.ShouldBe(@event.MerchantId);
        merchantSettlementFee.TransactionId.ShouldBe(@event.TransactionId);
        merchantSettlementFee.CalculatedValue.ShouldBe(@event.CalculatedValue);
        merchantSettlementFee.FeeCalculatedDateTime.ShouldBe(@event.FeeCalculatedDateTime);
        merchantSettlementFee.FeeValue.ShouldBe(@event.FeeValue);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_MerchantFeeAddedToTransactionEvent_MerchantFeeAdded(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantFeeAddedToTransactionEvent);
        MerchantFeeAddedToTransactionEvent @event = JsonConvert.DeserializeObject<MerchantFeeAddedToTransactionEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.AddSettledMerchantFeeToSettlement(TestData.SettlementId, @event, CancellationToken.None);

        MerchantSettlementFee merchantSettlementFee = await context.MerchantSettlementFees.SingleOrDefaultAsync(s => s.SettlementId == TestData.SettlementId
                                                                                                                    && s.EstateId == @event.EstateId &&
                                                                                                                    s.MerchantId == @event.MerchantId &&
                                                                                                                    s.FeeId == @event.FeeId);
        merchantSettlementFee.ShouldNotBeNull();
        merchantSettlementFee.EstateId.ShouldBe(@event.EstateId);
        merchantSettlementFee.SettlementId.ShouldBe(TestData.SettlementId);
        merchantSettlementFee.FeeId.ShouldBe(@event.FeeId);
        merchantSettlementFee.IsSettled.ShouldBeTrue();
        merchantSettlementFee.MerchantId.ShouldBe(@event.MerchantId);
        merchantSettlementFee.TransactionId.ShouldBe(@event.TransactionId);
        merchantSettlementFee.CalculatedValue.ShouldBe(@event.CalculatedValue);
        merchantSettlementFee.FeeCalculatedDateTime.ShouldBe(@event.FeeCalculatedDateTime);
        merchantSettlementFee.FeeValue.ShouldBe(@event.FeeValue);
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_MarkMerchantFeeAsSettled_FeeMarkedAsSettled(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        context.MerchantSettlementFees.Add(new MerchantSettlementFee
                                           {
                                               SettlementId = TestData.SettlementId,
                                               EstateId = TestData.EstateId,
                                               FeeId = TestData.TransactionFeeId,
                                               MerchantId = TestData.MerchantId,
                                               TransactionId = TestData.TransactionId,
                                               FeeValue = TestData.FeeValue,
                                               CalculatedValue = TestData.CalculatedValue,
                                               FeeCalculatedDateTime = TestData.FeeCalculatedDateTime,
                                               IsSettled = false
                                           });
        context.SaveChanges();
        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantFeeSettledEvent);
        MerchantFeeSettledEvent @event = JsonConvert.DeserializeObject<MerchantFeeSettledEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.MarkMerchantFeeAsSettled(@event, CancellationToken.None);

        MerchantSettlementFee merchantSettlementFee = await context.MerchantSettlementFees.SingleOrDefaultAsync(s => s.SettlementId == @event.SettlementId &&
                                                                                                                    s.EstateId == @event.EstateId &&
                                                                                                                    s.MerchantId == @event.MerchantId &&
                                                                                                                    s.FeeId == @event.FeeId);
        merchantSettlementFee.ShouldNotBeNull();
        merchantSettlementFee.IsSettled.ShouldBeTrue();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_MarkMerchantFeeAsSettled_FeeNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.MerchantFeeSettledEvent);
        MerchantFeeSettledEvent @event = JsonConvert.DeserializeObject<MerchantFeeSettledEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.MarkMerchantFeeAsSettled(@event, CancellationToken.None);
                                        });
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_MarkSettlementAsCompleted_SettlementMarkedAsComplete(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
        context.Settlements.Add(new Settlement
                                {
                                    SettlementId = TestData.SettlementId,
                                    EstateId = TestData.EstateId,
                                    IsCompleted = false,
                                    SettlementDate = TestData.SettlementDate
                                });
        context.SaveChanges();
        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.SettlementCompletedEvent);
        SettlementCompletedEvent @event = JsonConvert.DeserializeObject<SettlementCompletedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);

        await reportingRepository.MarkSettlementAsCompleted(@event, CancellationToken.None);

        Settlement settlement = await context.Settlements.SingleOrDefaultAsync(s => s.SettlementId == @event.SettlementId &&
                                                                                    s.EstateId == @event.EstateId);
        settlement.ShouldNotBeNull();
        settlement.IsCompleted.ShouldBeTrue();
    }

    [Theory]
    [InlineData(TestDatabaseType.InMemory)]
    [InlineData(TestDatabaseType.SqliteInMemory)]
    public async Task EstateReportingRepository_MarkSettlementAsCompleted_SettlementNotFOund_ErrorThrown(TestDatabaseType testDatabaseType)
    {
        EstateReportingGenericContext context = await this.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

        Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> dbContextFactory = this.CreateMockContextFactory();
        dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

        String jsonData = JsonConvert.SerializeObject(TestData.SettlementCompletedEvent);
        SettlementCompletedEvent @event = JsonConvert.DeserializeObject<SettlementCompletedEvent>(jsonData);

        EstateReportingRepository reportingRepository = new EstateReportingRepository(dbContextFactory.Object);
        Should.Throw<NotFoundException>(async () =>
                                        {
                                            await reportingRepository.MarkSettlementAsCompleted(@event, CancellationToken.None);

                                        });
    }

    private async Task<EstateReportingGenericContext> GetContext(String databaseName, TestDatabaseType databaseType = TestDatabaseType.InMemory)
    {
        EstateReportingGenericContext context = null;
        if (databaseType == TestDatabaseType.InMemory)
        {
            DbContextOptionsBuilder<EstateReportingGenericContext> builder = new DbContextOptionsBuilder<EstateReportingGenericContext>()
                                                                             .UseInMemoryDatabase(databaseName, b=> b.EnableNullChecks(false))
                                                                             .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            context = new EstateReportingSqlServerContext(builder.Options);
        }
        else if (databaseType == TestDatabaseType.SqliteInMemory)
        {
            SqliteConnection inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            inMemorySqlite.Open();

            DbContextOptionsBuilder<EstateReportingGenericContext> builder = new DbContextOptionsBuilder<EstateReportingGenericContext>().UseSqlite(inMemorySqlite);
            context = new EstateReportingSqlServerContext(builder.Options);
            await context.Database.EnsureCreatedAsync();
        }
        else
        {
            throw new NotSupportedException($"Database type [{databaseType}] not supported");
        }

        return context;
    }



    public enum TestDatabaseType
    {
        InMemory = 0,
        SqliteInMemory = 1
    }
}