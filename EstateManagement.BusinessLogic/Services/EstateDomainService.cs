namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IEstateDomainService" />
    public class EstateDomainService : IEstateDomainService
    {
        #region Fields

        /// <summary>
        /// The aggregate repository manager
        /// </summary>
        private readonly IAggregateRepositoryManager AggregateRepositoryManager;

        /// <summary>
        /// The security service client
        /// </summary>
        private readonly ISecurityServiceClient SecurityServiceClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateDomainService" /> class.
        /// </summary>
        /// <param name="aggregateRepositoryManager">The aggregate repository manager.</param>
        /// <param name="securityServiceClient">The security service client.</param>
        public EstateDomainService(IAggregateRepositoryManager aggregateRepositoryManager,
                                   ISecurityServiceClient securityServiceClient)
        {
            this.AggregateRepositoryManager = aggregateRepositoryManager;
            this.SecurityServiceClient = securityServiceClient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="estateName">Name of the estate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task CreateEstate(Guid estateId,
                                       String estateName,
                                       CancellationToken cancellationToken)
        {
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);

            estateAggregate.Create(estateName);

            await estateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);
        }

        /// <summary>
        /// Creates the operator.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="operatorName">Name of the operator.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddOperatorToEstate(Guid estateId,
                                         Guid operatorId,
                                         String operatorName,
                                         Boolean requireCustomMerchantNumber,
                                         Boolean requireCustomTerminalNumber,
                                         CancellationToken cancellationToken)
        {
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);

            estateAggregate.AddOperator(operatorId, operatorName, requireCustomMerchantNumber, requireCustomTerminalNumber);

            await estateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);
        }

        /// <summary>
        /// Creates the estate user.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="password">The password.</param>
        /// <param name="givenName">Name of the given.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="familyName">Name of the family.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Guid> CreateEstateUser(Guid estateId,
                                           String emailAddress,
                                           String password,
                                           String givenName,
                                           String middleName,
                                           String familyName,
                                           CancellationToken cancellationToken)
        {
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);

            CreateUserRequest createUserRequest = new CreateUserRequest
                                                  {
                                                      EmailAddress = emailAddress,
                                                      FamilyName = familyName,
                                                      GivenName = givenName,
                                                      MiddleName = middleName,
                                                      Password = password,
                                                      PhoneNumber = "123456", // Is this really needed :|
                                                      Roles = new List<String>(),
                                                      Claims = new Dictionary<String, String>()
                                                  };

            createUserRequest.Roles.Add("Estate");
            createUserRequest.Claims.Add("EstateId", estateId.ToString());
            
            CreateUserResponse createUserResponse = await this.SecurityServiceClient.CreateUser(createUserRequest, cancellationToken);

            // Add the user to the aggregate 
            estateAggregate.AddSecurityUser(createUserResponse.UserId, emailAddress);

            // TODO: add a delete user here in case the aggregate add fails...

            await estateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);

            return createUserResponse.UserId;
        }

        #endregion
    }
}