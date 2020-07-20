namespace EstateManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
    using AddMerchantDeviceRequest = BusinessLogic.Requests.AddMerchantDeviceRequest;
    using AddMerchantDeviceRequestDTO = DataTransferObjects.Requests.AddMerchantDeviceRequest;
    using MakeMerchantDepositRequest = BusinessLogic.Requests.MakeMerchantDepositRequest;
    using MakeMerchantDepositRequestDTO = DataTransferObjects.Requests.MakeMerchantDepositRequest;
    using EstateManagement.Common;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Models.Contract;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ExcludeFromCodeCoverage]
    [Route(MerchantController.ControllerRoute)]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
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
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { String.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

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
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Merchant not found with estate Id {estateId} and merchant Id {merchantId}</exception>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetMerchants([FromRoute] Guid estateId, CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { String.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            List<Merchant> merchants = await this.EstateManagementManager.GetMerchants(estateId, cancellationToken);

            if (merchants == null || merchants.Any() == false)
            {
                throw new NotFoundException($"No Merchants found for estate Id {estateId}");
            }

            return this.Ok(this.ModelFactory.ConvertFrom(merchants));
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
            String estateRoleName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("EstateRoleName")) ? "Estate" : Environment.GetEnvironmentVariable("EstateRoleName");
            String merchantRoleName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("MerchantRoleName")) ? "Merchant" : Environment.GetEnvironmentVariable("MerchantRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[]
                                                         {
                                                             estateRoleName,
                                                             merchantRoleName
                                                         }) == false)
            {
                return this.Forbid();
            }

            Claim estateIdClaim = null;
            Claim merchantIdClaim = null;

            // Determine the users role
            if (this.User.IsInRole(estateRoleName))
            {
                // Estate user
                // Get the Estate Id claim from the user
                estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId");
            }

            if (this.User.IsInRole(merchantRoleName))
            {
                // Get the merchant Id claim from the user
                estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId");
                merchantIdClaim = ClaimsHelper.GetUserClaim(this.User, "MerchantId");
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(merchantId, merchantIdClaim) == false)
            {
                return this.Forbid();
            }

            Merchant merchant = await this.EstateManagementManager.GetMerchant(estateId, merchantId, cancellationToken);

            if (merchant == null)
            {
                throw new NotFoundException($"Merchant not found with estate Id {estateId} and merchant Id {merchantId}");
            }

            MerchantBalance merchantBalance = await this.EstateManagementManager.GetMerchantBalance(estateId, merchantId, cancellationToken);

            if (merchantBalance == null)
            {
                throw new NotFoundException($"Merchant Balance details not found with estate Id {estateId} and merchant Id {merchantId}");
            }

            return this.Ok(this.ModelFactory.ConvertFrom(merchant, merchantBalance));
        }

        /// <summary>
        /// Gets the merchant balance.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Merchant Balance details not found with estate Id {estateId} and merchant Id {merchantId}</exception>
        [HttpGet]
        [Route("{merchantId}/balance")]
        public async Task<IActionResult> GetMerchantBalance([FromRoute] Guid estateId, [FromRoute] Guid merchantId, CancellationToken cancellationToken)
        {
            String estateRoleName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("EstateRoleName")) ? "Estate" : Environment.GetEnvironmentVariable("EstateRoleName");
            String merchantRoleName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("MerchantRoleName")) ? "Merchant" : Environment.GetEnvironmentVariable("MerchantRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[]
                                                         {
                                                             estateRoleName,
                                                             merchantRoleName
                                                         }) == false)
            {
                return this.Forbid();
            }

            Claim estateIdClaim = null;
            Claim merchantIdClaim = null;

            // Determine the users role
            if (this.User.IsInRole(estateRoleName))
            {
                // Estate user
                // Get the Estate Id claim from the user
                estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId");
            }

            if (this.User.IsInRole(merchantRoleName))
            {
                // Get the merchant Id claim from the user
                estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId");
                merchantIdClaim = ClaimsHelper.GetUserClaim(this.User, "MerchantId");
            }
            
            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(merchantId, merchantIdClaim) == false)
            {
                return this.Forbid();
            }

            MerchantBalance merchantBalance = await this.EstateManagementManager.GetMerchantBalance(estateId, merchantId, cancellationToken);
            
            if (merchantBalance == null)
            {
                throw new NotFoundException($"Merchant Balance details not found with estate Id {estateId} and merchant Id {merchantId}");
            }

            return this.Ok(this.ModelFactory.ConvertFrom(merchantBalance));
        }

        [HttpPost]
        [Route("{merchantId}/deposits")]
        public async Task<IActionResult> MakeDeposit([FromRoute] Guid estateId,
                                                     [FromRoute] Guid merchantId,
                                                     [FromBody] MakeMerchantDepositRequestDTO makeMerchantDepositRequest,
                                                     CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { String.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            // Convert the source
            Models.MerchantDepositSource merchantDepositSourceModel = Models.MerchantDepositSource.NotSet;
            switch(makeMerchantDepositRequest.Source)
            {
                case MerchantDepositSource.Manual:
                    merchantDepositSourceModel = Models.MerchantDepositSource.Manual;
                    break;
                    case MerchantDepositSource.Automatic:
                        merchantDepositSourceModel = Models.MerchantDepositSource.Automatic;
                        break;
                    default:
                        merchantDepositSourceModel = Models.MerchantDepositSource.Manual;
                        break;
            }

            MakeMerchantDepositRequest command = MakeMerchantDepositRequest.Create(estateId,
                                                                                   merchantId,
                                                                                   merchantDepositSourceModel,
                                                                                   makeMerchantDepositRequest.Reference,
                                                                                   makeMerchantDepositRequest.DepositDateTime,
                                                                                   makeMerchantDepositRequest.Amount);

            // Route the command
            Guid depositId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new MakeMerchantDepositResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    DepositId = depositId
                                });

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
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { String.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

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
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { String.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

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

        [HttpPost]
        [Route("{merchantId}/devices")]
        public async Task<IActionResult> AddDevice([FromRoute] Guid estateId,
                                                    [FromRoute] Guid merchantId,
                                                    [FromBody] AddMerchantDeviceRequestDTO addMerchantDeviceRequest,
                                                    CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { String.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            Guid deviceId = Guid.NewGuid();

            AddMerchantDeviceRequest command = AddMerchantDeviceRequest.Create(estateId, merchantId, deviceId, addMerchantDeviceRequest.DeviceIdentifier);
            
            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new AddMerchantDeviceResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    DeviceId = deviceId
                                });
        }

        #endregion
        [Route("{merchantId}/contracts/{contractId}/products/{productId}/transactionFees")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionFeesForProduct([FromRoute] Guid estateId,
                                                                      [FromRoute] Guid merchantId,
                                                                      [FromRoute] Guid contractId,
                                                                      [FromRoute] Guid productId,
                                                                      CancellationToken cancellationToken)
        {
            String estateRoleName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("EstateRoleName")) ? "Estate" : Environment.GetEnvironmentVariable("EstateRoleName");
            String merchantRoleName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("MerchantRoleName")) ? "Merchant" : Environment.GetEnvironmentVariable("MerchantRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[]
                                                         {
                                                             estateRoleName,
                                                             merchantRoleName
                                                         }) == false)
            {
                return this.Forbid();
            }

            Claim estateIdClaim = null;
            Claim merchantIdClaim = null;

            // Determine the users role
            if (this.User.IsInRole(estateRoleName))
            {
                // Estate user
                // Get the Estate Id claim from the user
                estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId");
            }

            if (this.User.IsInRole(merchantRoleName))
            {
                // Get the merchant Id claim from the user
                estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId");
                merchantIdClaim = ClaimsHelper.GetUserClaim(this.User, "MerchantId");
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(merchantId, merchantIdClaim) == false)
            {
                return this.Forbid();
            }

            List<TransactionFee> transactionFees = await this.EstateManagementManager.GetTransactionFeesForProduct(estateId, merchantId, contractId, productId, cancellationToken);

            return this.Ok(this.ModelFactory.ConvertFrom(transactionFees));
        }

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