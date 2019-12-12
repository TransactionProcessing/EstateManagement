namespace EstateManagement.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Manger;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis.Operations;
    using Models.Merchant;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.Exceptions;
    using CreateMerchantRequest = BusinessLogic.Requests.CreateMerchantRequest;
    using CreateMerchantRequestDTO = DataTransferObjects.Requests.CreateMerchantRequest;
    using AssignOperatorToMerchantRequest = BusinessLogic.Requests.AssignOperatorToMerchantRequest;
    using AssignOperatorRequestDTO = DataTransferObjects.Requests.AssignOperatorRequest;
    using CreateMerchantUserRequest = BusinessLogic.Requests.CreateMerchantUserRequest;
    using CreateMerchantUserRequestDTO = DataTransferObjects.Requests.CreateMerchantUserRequest;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ExcludeFromCodeCoverage]
    [Route(MerchantController.ControllerRoute)]
    [ApiController]
    [ApiVersion("1.0")]
    public class MerchantController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        /// <summary>
        /// The estate management manager
        /// </summary>
        private readonly IEstateManagementManager EstateManagementManager;

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateController" /> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="estateManagementManager">The estate management manager.</param>
        /// <param name="modelFactory">The model factory.</param>
        public MerchantController(IMediator mediator,
                                  IEstateManagementManager estateManagementManager,
                                  IModelFactory modelFactory)
        {
            this.Mediator = mediator;
            this.EstateManagementManager = estateManagementManager;
            this.ModelFactory = modelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createMerchantRequest">The create merchant request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateMerchant([FromRoute] Guid estateId,
                                                        [FromBody] CreateMerchantRequestDTO createMerchantRequest,
                                                        CancellationToken cancellationToken)
        {
            Guid merchantId = Guid.NewGuid();

            // Create the command
            CreateMerchantRequest command = CreateMerchantRequest.Create(estateId,
                                                                         merchantId,
                                                                         createMerchantRequest.Name,
                                                                         createMerchantRequest.Address.AddressLine1,
                                                                         createMerchantRequest.Address.AddressLine2,
                                                                         createMerchantRequest.Address.AddressLine3,
                                                                         createMerchantRequest.Address.AddressLine4,
                                                                         createMerchantRequest.Address.Town,
                                                                         createMerchantRequest.Address.Region,
                                                                         createMerchantRequest.Address.PostalCode,
                                                                         createMerchantRequest.Address.Country,
                                                                         createMerchantRequest.Contact.ContactName,
                                                                         createMerchantRequest.Contact.PhoneNumber,
                                                                         createMerchantRequest.Contact.EmailAddress);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new CreateMerchantResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    AddressId = command.AddressId,
                                    ContactId = command.ContactId
                                });
        }

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Merchant not found with estate Id {estateId} and merchant Id {merchantId}</exception>
        [HttpGet]
        [Route("{merchantId}")]
        public async Task<IActionResult> GetMerchant([FromRoute] Guid estateId, [FromRoute] Guid merchantId, CancellationToken cancellationToken)
        {
            Merchant merchant = await this.EstateManagementManager.GetMerchant(estateId, merchantId, cancellationToken);

            if (merchant == null)
            {
                throw new NotFoundException($"Merchant not found with estate Id {estateId} and merchant Id {merchantId}");
            }

            return this.Ok(this.ModelFactory.ConvertFrom(merchant));
        }

        /// <summary>
        /// Assigns the operator.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="assignOperatorRequest">The assign operator request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{merchantId}/operators")]
        public async Task<IActionResult> AssignOperator([FromRoute] Guid estateId,
                                                        [FromRoute] Guid merchantId,
                                                        AssignOperatorRequestDTO assignOperatorRequest,
                                                        CancellationToken cancellationToken)
        {
            AssignOperatorToMerchantRequest command = AssignOperatorToMerchantRequest.Create(estateId, merchantId,assignOperatorRequest.OperatorId,
                                                                                             assignOperatorRequest.MerchantNumber, assignOperatorRequest.TerminalNumber);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new AssignOperatorResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    OperatorId = assignOperatorRequest.OperatorId
                                });
        }

        [HttpPost]
        [Route("{merchantId}/users")]
        public async Task<IActionResult> CreateMerchantUser([FromRoute] Guid estateId, 
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] CreateMerchantUserRequestDTO createMerchantUserRequest,
                                                            CancellationToken cancellationToken)
        {
            // Create the command
            CreateMerchantUserRequest request = CreateMerchantUserRequest.Create(estateId, merchantId, createMerchantUserRequest.EmailAddress,
                                                                                 createMerchantUserRequest.Password,
                                                                                 createMerchantUserRequest.GivenName,
                                                                                 createMerchantUserRequest.MiddleName,
                                                                                 createMerchantUserRequest.FamilyName);

            // Route the command
            Guid userId = await this.Mediator.Send(request, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}/users/{userId}",
                                new CreateMerchantUserResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    UserId = userId
                                });
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "merchants";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/estates/{estateid}/" + MerchantController.ControllerName;

        #endregion
    }
}