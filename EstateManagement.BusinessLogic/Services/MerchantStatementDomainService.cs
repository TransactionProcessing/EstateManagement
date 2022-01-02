﻿namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using EstateAggregate;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using MessagingService.Client;
    using MessagingService.DataTransferObjects;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
    using Models.MerchantStatement;
    using Repository;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;
    using Shared.Logger;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IMerchantStatementDomainService" />
    public class MerchantStatementDomainService : IMerchantStatementDomainService
    {
        #region Fields

        /// <summary>
        /// The merchant aggregate repository
        /// </summary>
        private readonly IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent> MerchantAggregateRepository;

        /// <summary>
        /// The merchant statement aggregate repository
        /// </summary>
        private readonly IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent> MerchantStatementAggregateRepository;

        private readonly IAggregateRepository<EstateAggregate, DomainEventRecord.DomainEvent> EstateAggregateRepository;

        private readonly IEstateManagementRepository EstateManagementRepository;

        private readonly IStatementBuilder StatementBuilder;

        private readonly IMessagingServiceClient MessagingServiceClient;

        private readonly ISecurityServiceClient SecurityServiceClient;

        private readonly IFileSystem FileSystem;

        private readonly IPDFGenerator PdfGenerator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantStatementDomainService" /> class.
        /// </summary>
        /// <param name="merchantAggregateRepository">The merchant aggregate repository.</param>
        /// <param name="merchantStatementAggregateRepository">The merchant statement aggregate repository.</param>
        /// <param name="estateManagementRepository">The estate management repository.</param>
        /// <param name="statementBuilder">The statement builder.</param>
        /// <param name="messagingServiceClient">The messaging service client.</param>
        /// <param name="securityServiceClient">The security service client.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="pdfGenerator">The PDF generator.</param>
        public MerchantStatementDomainService(IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent> merchantAggregateRepository,
                                              IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent> merchantStatementAggregateRepository,
                                              IEstateManagementRepository estateManagementRepository,
                                              IStatementBuilder statementBuilder,
                                              IMessagingServiceClient messagingServiceClient,
                                              ISecurityServiceClient securityServiceClient,
                                              IFileSystem fileSystem,
                                              IPDFGenerator pdfGenerator)
        {
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.MerchantStatementAggregateRepository = merchantStatementAggregateRepository;
            this.EstateManagementRepository = estateManagementRepository;
            this.StatementBuilder = statementBuilder;
            this.MessagingServiceClient = messagingServiceClient;
            this.SecurityServiceClient = securityServiceClient;
            this.FileSystem = fileSystem;
            this.PdfGenerator = pdfGenerator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the settled fee to statement.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="settledDateTime">The settled date time.</param>
        /// <param name="settledAmount">The settled amount.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="settledFeeId">The settled fee identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddSettledFeeToStatement(Guid estateId,
                                                   Guid merchantId,
                                                   DateTime settledDateTime,
                                                   Decimal settledAmount,
                                                   Guid transactionId,
                                                   Guid settledFeeId,
                                                   CancellationToken cancellationToken)
        {
            // Work out the next statement date
            DateTime nextStatementDate = CalculateStatementDate(settledDateTime);

            Guid statementId = GuidCalculator.Combine(merchantId, nextStatementDate.ToGuid());
            Guid settlementFeeId = GuidCalculator.Combine(transactionId, settledFeeId);
            MerchantStatementAggregate merchantStatementAggregate =
                await this.MerchantStatementAggregateRepository.GetLatestVersion(statementId, cancellationToken);
            
            MerchantStatement merchantStatement = merchantStatementAggregate.GetStatement();
            if (merchantStatement.IsCreated == false)
            {
                merchantStatementAggregate.CreateStatement(estateId, merchantId, nextStatementDate);
            }

            // Add settled fee to statement
            SettledFee settledFee = new SettledFee
                                    {
                                        DateTime = settledDateTime,
                                        Amount = settledAmount,
                                        TransactionId = transactionId,
                                        SettledFeeId = settlementFeeId
            };

            merchantStatementAggregate.AddSettledFeeToStatement(settledFee);

            await this.MerchantStatementAggregateRepository.SaveChanges(merchantStatementAggregate, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventDateTime"></param>
        /// <returns></returns>
        internal static DateTime CalculateStatementDate(DateTime eventDateTime)
        {
            var calculatedDateTime = eventDateTime.Date.AddMonths(1);

            return new DateTime(calculatedDateTime.Year, calculatedDateTime.Month, 1);
        }

        /// <summary>
        /// Generates the statement.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="statementDate">The statement date.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Guid> GenerateStatement(Guid estateId,
                                                  Guid merchantId,
                                                  DateTime statementDate,
                                                  CancellationToken cancellationToken)
        {
            Guid statementId = GuidCalculator.Combine(merchantId, statementDate.ToGuid());
            MerchantStatementAggregate merchantStatementAggregate =
                await this.MerchantStatementAggregateRepository.GetLatestVersion(statementId, cancellationToken);

            merchantStatementAggregate.GenerateStatement(DateTime.Now);

            await this.MerchantStatementAggregateRepository.SaveChanges(merchantStatementAggregate, cancellationToken);

            return merchantStatementAggregate.AggregateId;
        }

        public async Task EmailStatement(Guid estateId,
                                         Guid merchantId,
                                         Guid merchantStatementId,
                                         CancellationToken cancellationToken)
        {
            StatementHeader statementHeader = await this.EstateManagementRepository.GetStatement(estateId, merchantStatementId, cancellationToken);
            
            String html = await this.StatementBuilder.GetStatementHtml(statementHeader, cancellationToken);

            String base64 = await this.PdfGenerator.CreatePDF(html, cancellationToken);

            SendEmailRequest sendEmailRequest = new SendEmailRequest
            {
                Body = "<html><body>Please find attached this months statement.</body></html>",
                ConnectionIdentifier = estateId,
                FromAddress = "golfhandicapping@btinternet.com", // TODO: lookup from config
                IsHtml = true,
                Subject = $"Merchant Statement for {statementHeader.StatementDate}",
                MessageId = merchantStatementId,
                ToAddresses = new List<String>
                              {
                                  statementHeader.MerchantEmail
                              },
                EmailAttachments = new List<EmailAttachment>
                                   {
                                       new EmailAttachment
                                       {
                                           FileData = base64,
                                           FileType = FileType.PDF,
                                           Filename = $"merchantstatement{statementHeader.StatementDate}.pdf"
                                       }
                                   }
            };

            this.TokenResponse = await this.GetToken(cancellationToken);

            SendEmailResponse sendEmailResponse = await this.MessagingServiceClient.SendEmail(this.TokenResponse.AccessToken, sendEmailRequest, cancellationToken);

            // record email getting sent in statement aggregate
            MerchantStatementAggregate merchantStatementAggregate =
                await this.MerchantStatementAggregateRepository.GetLatestVersion(merchantStatementId, cancellationToken);

            merchantStatementAggregate.EmailStatement(DateTime.Now, sendEmailResponse.MessageId);

            await this.MerchantStatementAggregateRepository.SaveChanges(merchantStatementAggregate, cancellationToken);
        }

        /// <summary>
        /// The token response
        /// </summary>
        private TokenResponse TokenResponse;

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private async Task<TokenResponse> GetToken(CancellationToken cancellationToken)
        {
            // Get a token to talk to the estate service
            String clientId = ConfigurationReader.GetValue("AppSettings", "ClientId");
            String clientSecret = ConfigurationReader.GetValue("AppSettings", "ClientSecret");
            Logger.LogInformation($"Client Id is {clientId}");
            Logger.LogInformation($"Client Secret is {clientSecret}");

            if (this.TokenResponse == null)
            {
                TokenResponse token = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
                Logger.LogInformation($"Token is {token.AccessToken}");
                return token;
            }

            if (this.TokenResponse.Expires.UtcDateTime.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(2))
            {
                Logger.LogInformation($"Token is about to expire at {this.TokenResponse.Expires.DateTime:O}");
                TokenResponse token = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
                Logger.LogInformation($"Token is {token.AccessToken}");
                return token;
            }

            return this.TokenResponse;
        }

        /// <summary>
        /// Adds the transaction to statement.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="transactionDateTime">The transaction date time.</param>
        /// <param name="transactionAmount">The transaction amount.</param>
        /// <param name="isAuthorised">if set to <c>true</c> [is authorised].</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddTransactionToStatement(Guid estateId,
                                                    Guid merchantId,
                                                    DateTime transactionDateTime,
                                                    Decimal? transactionAmount,
                                                    Boolean isAuthorised,
                                                    Guid transactionId,
                                                    CancellationToken cancellationToken)
        {
            // Transaction Completed arrives(if this is a logon transaction or failed then return)
            if (isAuthorised == false)
                return;
            if (transactionAmount.HasValue == false)
                return;

            // Work out the next statement date
            DateTime nextStatementDate = CalculateStatementDate(transactionDateTime);

            Guid statementId = GuidCalculator.Combine(merchantId, nextStatementDate.ToGuid());

            MerchantStatementAggregate merchantStatementAggregate =
                await this.MerchantStatementAggregateRepository.GetLatestVersion(statementId, cancellationToken);
            MerchantStatement merchantStatement = merchantStatementAggregate.GetStatement();

            if (merchantStatement.IsCreated == false)
            {
                merchantStatementAggregate.CreateStatement(estateId, merchantId, nextStatementDate);
            }

            // Add transaction to statement
            Transaction transaction = new Transaction
            {
                DateTime = transactionDateTime,
                Amount = transactionAmount.Value,
                TransactionId = transactionId
            };

            merchantStatementAggregate.AddTransactionToStatement(transaction);

            await this.MerchantStatementAggregateRepository.SaveChanges(merchantStatementAggregate, cancellationToken);
        }

        #endregion
    }

    public static class GuidCalculator
    {
        #region Methods

        /// <summary>
        /// Combines the specified GUIDs into a new GUID.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid,
                                   Byte offset)
        {
            Byte[] firstAsBytes = firstGuid.ToByteArray();
            Byte[] secondAsBytes = secondGuid.ToByteArray();

            Byte[] newBytes = new Byte[16];

            for (Int32 i = 0; i < 16; i++)
            {
                // Add and truncate any overflow
                newBytes[i] = (Byte)(firstAsBytes[i] + secondAsBytes[i] + offset);
            }

            return new Guid(newBytes);
        }

        /// <summary>
        /// Combines the specified GUIDs into a new GUID.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid)
        {
            return GuidCalculator.Combine(firstGuid,
                                          secondGuid,
                                          0);
        }

        /// <summary>
        /// Combines the specified first unique identifier.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <param name="thirdGuid">The third unique identifier.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid,
                                   Guid thirdGuid,
                                   Byte offset)
        {
            Byte[] firstAsBytes = firstGuid.ToByteArray();
            Byte[] secondAsBytes = secondGuid.ToByteArray();
            Byte[] thirdAsBytes = thirdGuid.ToByteArray();

            Byte[] newBytes = new Byte[16];

            for (Int32 i = 0; i < 16; i++)
            {
                // Add and truncate any overflow
                newBytes[i] = (Byte)(firstAsBytes[i] + secondAsBytes[i] + thirdAsBytes[i] + offset);
            }

            return new Guid(newBytes);
        }

        /// <summary>
        /// Combines the specified first unique identifier.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <param name="thirdGuid">The third unique identifier.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid,
                                   Guid thirdGuid)
        {
            return GuidCalculator.Combine(firstGuid,
                                          secondGuid,
                                          thirdGuid,
                                          0);
        }

        #endregion
    }
}