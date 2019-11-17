namespace EstateManagement.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Commands;
    using BusinessLogic.Manger;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Factories;
    using Microsoft.AspNetCore.Mvc;
    using Shared.DomainDrivenDesign.CommandHandling;

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
        /// The command router
        /// </summary>
        private readonly ICommandRouter CommandRouter;

        /// <summary>
        /// The estate managment manager
        /// </summary>
        private readonly IEstateManagmentManager EstateManagmentManager;

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateController" /> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        /// <param name="estateManagmentManager">The estate managment manager.</param>
        /// <param name="modelFactory">The model factory.</param>
        public MerchantController(ICommandRouter commandRouter,
                                  IEstateManagmentManager estateManagmentManager,
                                  IModelFactory modelFactory)
        {
            this.CommandRouter = commandRouter;
            this.EstateManagmentManager = estateManagmentManager;
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
                                                        [FromBody] CreateMerchantRequest createMerchantRequest,
                                                        CancellationToken cancellationToken)
        {
            Guid merchantId = Guid.NewGuid();

            // Create the command
            CreateMerchantCommand command = CreateMerchantCommand.Create(estateId,
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
            await this.CommandRouter.Route(command, cancellationToken);

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