using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Services
{
    using System.Threading;
    using Common;
    using MediatR;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using Models.MerchantStatement;
    using Requests;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using TransactionProcessor.Transaction.DomainEvents;

    public interface IMerchantStatementDomainService
    {
        Task AddTransactionToStatement(Guid estateId,
                                       Guid merchantId,
                                       DateTime transactionDateTime,
                                       Decimal? transactionAmount,
                                       Boolean isAuthorised,
                                       Guid transactionId, 
                                       CancellationToken cancellationToken);
    }

    public class MerchantStatementDomainService : IMerchantStatementDomainService
    {
        private readonly IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent> MerchantAggregateRepository;

        private readonly IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent> MerchantStatementAggregateRepository;
        
        public MerchantStatementDomainService(IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent> merchantAggregateRepository,
                                              IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent> merchantStatementAggregateRepository)
        {
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.MerchantStatementAggregateRepository = merchantStatementAggregateRepository;
        }

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

            MerchantStatementAggregate merchantStatementAggregate = await this.MerchantStatementAggregateRepository.GetLatestVersion(nextStatementDate.ToGuid(), cancellationToken);
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
    }
}
