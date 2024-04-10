namespace EstateManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Manger;
    using BusinessLogic.Requests;
    using Common.Examples;
    using DataTransferObjects.Requests.Merchant;
    using DataTransferObjects.Responses.Contract;
    using DataTransferObjects.Responses.Merchant;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Contract;
    using Models.Merchant;
    using Shared.Exceptions;
    using Shared.General;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using GenerateMerchantStatementRequest = BusinessLogic.Requests.GenerateMerchantStatementRequest;
    using SwapMerchantDeviceRequestDTO = DataTransferObjects.Requests.Merchant.SwapMerchantDeviceRequest;
    using MakeMerchantDepositRequestDTO = DataTransferObjects.Requests.Merchant.MakeMerchantDepositRequest;
    using MakeMerchantWithdrawalRequestDTO = DataTransferObjects.Requests.Merchant.MakeMerchantWithdrawalRequest;
    using MerchantDepositSource = Models.MerchantDepositSource;
    using SwapMerchantDeviceRequest = BusinessLogic.Requests.SwapMerchantDeviceRequest;
    using GenerateMerchantStatementRequestDTO = DataTransferObjects.Requests.Merchant.GenerateMerchantStatementRequest;
    using AddMerchantContractRequestDTO = DataTransferObjects.Requests.Merchant.AddMerchantContractRequest;
    using Contract = Models.Contract.Contract;
    using SettlementSchedule = Models.SettlementSchedule;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ExcludeFromCodeCoverage]
    [Route(MerchantController.ControllerRoute)]
    [ApiController]
    [Authorize]
    public partial class MerchantController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The estate management manager
        /// </summary>
        private readonly IEstateManagementManager EstateManagementManager;

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateController" /> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="estateManagementManager">The estate management manager.</param>
        /// <param name="modelFactory">The model factory.</param>
        public MerchantController(IMediator mediator,
                                  IEstateManagementManager estateManagementManager)
        {
            this.Mediator = mediator;
            this.EstateManagementManager = estateManagementManager;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Generates the merchant statement.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="generateMerchantStatementRequest">The generate merchant statement request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{merchantId}/statements")]
        [SwaggerResponse(201, "Created", typeof(GenerateMerchantStatementResponse))]
        [SwaggerResponseExample(201, typeof(GenerateMerchantStatementResponseExample))]
        public async Task<IActionResult> GenerateMerchantStatement([FromRoute] Guid estateId,
                                                                   [FromRoute] Guid merchantId,
                                                                   [FromBody] GenerateMerchantStatementRequestDTO generateMerchantStatementRequest,
                                                                   CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] {string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName}) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            GenerateMerchantStatementRequest command = GenerateMerchantStatementRequest.Create(estateId, merchantId, generateMerchantStatementRequest.MerchantStatementDate);

            // Route the command
            Guid merchantStatementId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new GenerateMerchantStatementResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    MerchantStatementId = merchantStatementId
                                });
        }

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Shared.Exceptions.NotFoundException">
        /// Merchant not found with estate Id {estateId} and merchant Id {merchantId}
        /// or
        /// Merchant Balance details not found with estate Id {estateId} and merchant Id {merchantId}
        /// </exception>
        /// <exception cref="NotFoundException">Merchant not found with estate Id {estateId} and merchant Id {merchantId}</exception>
        [HttpGet]
        [Route("{merchantId}")]
        [SwaggerResponse(200, "Created", typeof(MerchantResponse))]
        [SwaggerResponseExample(200, typeof(MerchantResponseExample))]
        public async Task<IActionResult> GetMerchant([FromRoute] Guid estateId,
                                                     [FromRoute] Guid merchantId,
                                                     CancellationToken cancellationToken)
        {
            String estateRoleName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EstateRoleName"))
                ? "Estate"
                : Environment.GetEnvironmentVariable("EstateRoleName");
            String merchantRoleName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MerchantRoleName"))
                ? "Merchant"
                : Environment.GetEnvironmentVariable("MerchantRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] {estateRoleName, merchantRoleName}) == false)
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
            
            return this.Ok(ModelFactory.ConvertFrom(merchant));
        }
        
        /// <summary>
        /// Gets the merchant contracts.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [Route("{merchantId}/contracts")]
        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        [SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> GetMerchantContracts([FromRoute] Guid estateId,
                                                              [FromRoute] Guid merchantId,
                                                              CancellationToken cancellationToken)
        {
            String estateRoleName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EstateRoleName"))
                ? "Estate"
                : Environment.GetEnvironmentVariable("EstateRoleName");
            String merchantRoleName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MerchantRoleName"))
                ? "Merchant"
                : Environment.GetEnvironmentVariable("MerchantRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] {estateRoleName, merchantRoleName}) == false)
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

            List<Contract> contracts = await this.EstateManagementManager.GetMerchantContracts(estateId, merchantId, cancellationToken);

            return this.Ok(ModelFactory.ConvertFrom(contracts));
        }

        /// <summary>
        /// Gets the merchants.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Shared.Exceptions.NotFoundException">No Merchants found for estate Id {estateId}</exception>
        /// <exception cref="NotFoundException">No Merchants found for estate Id {estateId}</exception>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, "Created", typeof(List<MerchantResponse>))]
        [SwaggerResponseExample(200, typeof(MerchantResponseListExample))]
        public async Task<IActionResult> GetMerchants([FromRoute] Guid estateId,
                                                      CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] {string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName}) == false)
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

            return this.Ok(ModelFactory.ConvertFrom(merchants));
        }

        /// <summary>
        /// Gets the transaction fees for product.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [Route("{merchantId}/contracts/{contractId}/products/{productId}/transactionFees")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ContractProductTransactionFee>), 200)]
        [SwaggerResponse(200, "OK", typeof(List<ContractProductTransactionFee>))]
        [SwaggerResponseExample(200, typeof(ContractProductTransactionFeeResponseListExample))]
        public async Task<IActionResult> GetTransactionFeesForProduct([FromRoute] Guid estateId,
                                                                      [FromRoute] Guid merchantId,
                                                                      [FromRoute] Guid contractId,
                                                                      [FromRoute] Guid productId,
                                                                      CancellationToken cancellationToken)
        {
            String estateRoleName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EstateRoleName"))
                ? "Estate"
                : Environment.GetEnvironmentVariable("EstateRoleName");
            String merchantRoleName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MerchantRoleName"))
                ? "Merchant"
                : Environment.GetEnvironmentVariable("MerchantRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] {estateRoleName, merchantRoleName}) == false)
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

            List<TransactionFee> transactionFees =
                await this.EstateManagementManager.GetTransactionFeesForProduct(estateId, merchantId, contractId, productId, cancellationToken);

            return this.Ok(ModelFactory.ConvertFrom(transactionFees));
        }
        
        //[HttpPost]
        //[Route("{merchantId}/withdrawals")]
        ////[SwaggerResponse(201, "Created", typeof(MakeMerchantDepositResponse))]
        ////[SwaggerResponseExample(201, typeof(MakeMerchantDepositResponseExample))]
        //public async Task<IActionResult> MakeWithdrawal([FromRoute] Guid estateId,
        //                                             [FromRoute] Guid merchantId,
        //                                             [FromBody] MakeMerchantWithdrawalRequestDTO makeMerchantWithdrawalRequest,
        //                                             CancellationToken cancellationToken)
        //{
        //    // Get the Estate Id claim from the user
        //    Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

        //    String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
        //    if (ClaimsHelper.IsUserRolesValid(this.User, new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
        //    {
        //        return this.Forbid();
        //    }

        //    if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
        //    {
        //        return this.Forbid();
        //    }

        //    MakeMerchantWithdrawalRequest command = MakeMerchantWithdrawalRequest.Create(estateId,
        //                                                                           merchantId,
        //                                                                           makeMerchantWithdrawalRequest.WithdrawalDateTime,
        //                                                                           makeMerchantWithdrawalRequest.Amount);

        //    // Route the command
        //    Guid withdrawalId = await this.Mediator.Send(command, cancellationToken);

        //    // return the result
        //    return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
        //                        new MakeMerchantWithdrawalResponse()
        //                        {
        //                            EstateId = estateId,
        //                            MerchantId = merchantId,
        //                            WithdrawalId = withdrawalId
        //                        });
        //}

        /// <summary>
        /// Creates the merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="setSettlementScheduleRequest">The set settlement schedule request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{merchantId}")]
        //[SwaggerResponse(200, "Created", typeof(CreateMerchantResponse))]
        //[SwaggerResponseExample(201, typeof(CreateMerchantResponseExample))]
        public async Task<IActionResult> SetSettlementSchedule([FromRoute] Guid estateId,
                                                               [FromRoute] Guid merchantId,
                                                               [FromBody] SetSettlementScheduleRequest setSettlementScheduleRequest,
                                                               CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] {string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName}) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            // Convert the schedule
            SettlementSchedule settlementScheduleModel = SettlementSchedule.NotSet;
            switch(setSettlementScheduleRequest.SettlementSchedule)
            {
                case DataTransferObjects.Responses.Merchant.SettlementSchedule.Immediate:
                    settlementScheduleModel = SettlementSchedule.Immediate;
                    break;
                case DataTransferObjects.Responses.Merchant.SettlementSchedule.Weekly:
                    settlementScheduleModel = SettlementSchedule.Weekly;
                    break;
                case DataTransferObjects.Responses.Merchant.SettlementSchedule.Monthly:
                    settlementScheduleModel = SettlementSchedule.Monthly;
                    break;
                default:
                    settlementScheduleModel = SettlementSchedule.Immediate;
                    break;
            }

            SetMerchantSettlementScheduleRequest command = SetMerchantSettlementScheduleRequest.Create(estateId, merchantId, settlementScheduleModel);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Ok();
        }

        /// <summary>
        /// Swaps the merchant device.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="swapMerchantDeviceRequest">The swap merchant device request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{merchantId}/devices")]
        [SwaggerResponse(201, "Created", typeof(SwapMerchantDeviceResponse))]
        [SwaggerResponseExample(201, typeof(AddMerchantDeviceResponseExample))]
        public async Task<IActionResult> SwapMerchantDevice([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] SwapMerchantDeviceRequestDTO swapMerchantDeviceRequest,
                                                            CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] {string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName}) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            Guid deviceId = Guid.NewGuid();

            SwapMerchantDeviceRequest command = SwapMerchantDeviceRequest.Create(estateId,
                                                                                 merchantId,
                                                                                 deviceId,
                                                                                 swapMerchantDeviceRequest.OriginalDeviceIdentifier,
                                                                                 swapMerchantDeviceRequest.NewDeviceIdentifier);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new SwapMerchantDeviceResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    DeviceId = deviceId
                                });
        }

        //[HttpPatch]
        //[Route("{merchantId}")]
        ////[SwaggerResponse(201, "Created", typeof(SwapMerchantDeviceResponse))]
        ////[SwaggerResponseExample(201, typeof(AddMerchantDeviceResponseExample))]
        //public async Task<ActionResult> UpdateMerchant([FromRoute] Guid estateId,
        //                                               [FromRoute] Guid merchantId,
        //                                               [FromBody] UpdateMerchantRequestDTO updateMerchantRequest,
        //                                               CancellationToken cancellationToken){
            


        //    return this.NoContent();
        //}


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