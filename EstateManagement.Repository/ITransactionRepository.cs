using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransactionProcessor.Transaction.DomainEvents;

namespace EstateManagement.Repository
{
    public interface ITransactionRepository
    {
        Task StartTransaction(TransactionHasStartedEvent domainEvent, CancellationToken cancellationToken);

        //Task UpdateTransactionAuthorisation(TransactionAuthorisedByOperatorEvent domainEvent, CancellationToken cancellationToken);

        //Task UpdateTransactionAuthorisation(TransactionDeclinedByOperatorEvent domainEvent, CancellationToken cancellationToken);

        //Task UpdateTransactionAuthorisation(TransactionHasBeenLocallyAuthorisedEvent domainEvent, CancellationToken cancellationToken);

        //Task UpdateTransactionAuthorisation(TransactionHasBeenLocallyDeclinedEvent domainEvent, CancellationToken cancellationToken);

        //Task RecordTransactionAdditionalRequestData(AdditionalRequestDataRecordedEvent domainEvent, CancellationToken cancellationToken);

        //Task RecordTransactionAdditionalResponseData(AdditionalResponseDataRecordedEvent domainEvent, CancellationToken cancellationToken);

        //Task SetTransactionAmount(AdditionalRequestDataRecordedEvent domainEvent, CancellationToken cancellationToken); // TODO: is this function really needed.....

        //Task CompleteTransaction(TransactionHasBeenCompletedEvent domainEvent, CancellationToken cancellationToken);

        //Task AddSourceDetailsToTransaction(TransactionSourceAddedToTransactionEvent domainEvent, CancellationToken cancellationToken);

        //Task AddProductDetailsToTransaction(ProductDetailsAddedToTransactionEvent domainEvent, CancellationToken cancellationToken);
    }

    public class TransactionRepository : ITransactionRepository
    {
        public Task StartTransaction(TransactionHasStartedEvent domainEvent, CancellationToken cancellationToken)
        {
            // Resolve a context
            // Get the merchant details
            // Create a transaction entity
            // Add this to the context
            // Save the changes
            return Task.CompletedTask;
        }
    }
}
