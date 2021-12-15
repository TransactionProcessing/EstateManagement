namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Requests;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;EstateManagement.BusinessLogic.Requests.AddTransactionToMerchantStatementRequest&gt;" />
    public class MerchantStatementRequestHandler : IRequestHandler<AddTransactionToMerchantStatementRequest>
    {
        /// <summary>
        /// The domain service
        /// </summary>
        private readonly IMerchantStatementDomainService DomainService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantStatementRequestHandler"/> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        public MerchantStatementRequestHandler(IMerchantStatementDomainService domainService)
        {
            this.DomainService = domainService;
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Unit> Handle(AddTransactionToMerchantStatementRequest request,
                                       CancellationToken cancellationToken)
        {
            await this.DomainService.AddTransactionToStatement(request.EstateId,
                                                               request.MerchantId,
                                                               request.TransactionDateTime,
                                                               request.TransactionAmount,
                                                               request.IsAuthorised,
                                                               request.TransactionId,
                                                               cancellationToken);

            return new Unit();
        }
    }
}