namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using Models.MerchantStatement;
    using NLog;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;

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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantStatementDomainService" /> class.
        /// </summary>
        /// <param name="merchantAggregateRepository">The merchant aggregate repository.</param>
        /// <param name="merchantStatementAggregateRepository">The merchant statement aggregate repository.</param>
        public MerchantStatementDomainService(IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent> merchantAggregateRepository,
                                              IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent> merchantStatementAggregateRepository)
        {
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.MerchantStatementAggregateRepository = merchantStatementAggregateRepository;
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
            MerchantStatementAggregate merchantStatementAggregate =
                await this.MerchantStatementAggregateRepository.GetLatestVersion(statementDate.ToGuid(), cancellationToken);

            merchantStatementAggregate.GenerateStatement(DateTime.Now);

            await this.MerchantStatementAggregateRepository.SaveChanges(merchantStatementAggregate, cancellationToken);

            return merchantStatementAggregate.AggregateId;
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