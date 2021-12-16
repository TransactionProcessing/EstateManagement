namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using Models.MerchantStatement;
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
            // Merchant is rehydrated
            MerchantAggregate merchant = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);
            if (merchant.IsCreated == false)
                return;

            // Work out the next statement date (how is this done), do we feed statement generated events back into the merchant to update the statement date? Statements will be monthly!!
            // TODO: Statement date
            DateTime nextStatementDate = Guid.Parse("b5963507-c561-08d9-0000-000000000000").ToDateTime();

            MerchantStatementAggregate merchantStatementAggregate =
                await this.MerchantStatementAggregateRepository.GetLatestVersion(nextStatementDate.ToGuid(), cancellationToken);

            // Add settled fee to statement
            SettledFee settledFee = new SettledFee
                                    {
                                        DateTime = settledDateTime,
                                        Amount = settledAmount,
                                        TransactionId = transactionId,
                                        SettledFeeId = settledFeeId
                                    };

            merchantStatementAggregate.AddSettledFeeToStatement(settledFee);

            await this.MerchantStatementAggregateRepository.SaveChanges(merchantStatementAggregate, cancellationToken);
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
            // TODO: Move to domain service
            // Transaction Completed arrives (if this is a logon transaction or failed then return)
            if (isAuthorised == false)
                return;
            if (transactionAmount.HasValue == false)
                return;

            // Merchant is rehydrated
            MerchantAggregate merchant = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);
            if (merchant.IsCreated == false)
                return;

            // Work out the next statement date (how is this done), do we feed statement generated events back into the merchant to update the statement date? Statements will be monthly!!
            // TODO: Statement date
            DateTime nextStatementDate = DateTime.Now.AddDays(7);

            MerchantStatementAggregate merchantStatementAggregate =
                await this.MerchantStatementAggregateRepository.GetLatestVersion(nextStatementDate.ToGuid(), cancellationToken);
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
}