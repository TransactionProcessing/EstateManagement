namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Requests;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;EstateManagement.BusinessLogic.Requests.AddTransactionToMerchantStatementRequest&gt;" />
    /// <seealso cref="MediatR.IRequestHandler&lt;EstateManagement.BusinessLogic.Requests.AddSettledFeeToMerchantStatementRequest&gt;" />
    /// <seealso cref="MediatR.IRequestHandler&lt;EstateManagement.BusinessLogic.Requests.AddTransactionToMerchantStatementRequest&gt;" />
    public class MerchantStatementRequestHandler : IRequestHandler<AddTransactionToMerchantStatementRequest>, 
                                                   IRequestHandler<AddSettledFeeToMerchantStatementRequest>,
                                                   IRequestHandler<MerchantCommands.GenerateMerchantStatementCommand, Guid>,
                                                   IRequestHandler<EmailMerchantStatementRequest>
    {
        #region Fields

        /// <summary>
        /// The domain service
        /// </summary>
        private readonly IMerchantStatementDomainService MerchantStatementDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantStatementRequestHandler" /> class.
        /// </summary>
        /// <param name="merchantStatementDomainService">The merchant statement domain service.</param>
        public MerchantStatementRequestHandler(IMerchantStatementDomainService merchantStatementDomainService)
        {
            this.MerchantStatementDomainService = merchantStatementDomainService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task Handle(AddTransactionToMerchantStatementRequest request,
                                       CancellationToken cancellationToken)
        {
            await this.MerchantStatementDomainService.AddTransactionToStatement(request.EstateId,
                                                                                request.MerchantId,
                                                                                request.TransactionDateTime,
                                                                                request.TransactionAmount,
                                                                                request.IsAuthorised,
                                                                                request.TransactionId,
                                                                                cancellationToken);
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task Handle(AddSettledFeeToMerchantStatementRequest request,
                                       CancellationToken cancellationToken)
        {
            await this.MerchantStatementDomainService.AddSettledFeeToStatement(request.EstateId,
                                                                               request.MerchantId,
                                                                               request.SettledDateTime,
                                                                               request.SettledAmount,
                                                                               request.TransactionId,
                                                                               request.SettledFeeId,
                                                                               cancellationToken);
        }

        #endregion

        public async Task<Guid> Handle(MerchantCommands.GenerateMerchantStatementCommand command, CancellationToken cancellationToken)
        {
            return await this.MerchantStatementDomainService.GenerateStatement(command, cancellationToken);
        }

        public async Task Handle(EmailMerchantStatementRequest request,
                                       CancellationToken cancellationToken)
        {
            await this.MerchantStatementDomainService.EmailStatement(request.EstateId, request.MerchantId, request.MerchantStatementId, cancellationToken);
        }
    }
}