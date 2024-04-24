namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using Requests;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IEstateDomainService" />
    public class EstateDomainService : IEstateDomainService
    {
        #region Fields

        /// <summary>
        /// The estate aggregate repository
        /// </summary>
        private readonly IAggregateRepository<EstateAggregate, DomainEvent> EstateAggregateRepository;

        /// <summary>
        /// The security service client
        /// </summary>
        private readonly ISecurityServiceClient SecurityServiceClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateDomainService" /> class.
        /// </summary>
        /// <param name="estateAggregateRepository">The estate aggregate repository.</param>
        /// <param name="securityServiceClient">The security service client.</param>
        public EstateDomainService(IAggregateRepository<EstateAggregate, DomainEvent> estateAggregateRepository,
                                   ISecurityServiceClient securityServiceClient)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
            this.SecurityServiceClient = securityServiceClient;
        }

        #endregion

        #region Methods

        public async Task CreateEstate(EstateCommands.CreateEstateCommand command, CancellationToken cancellationToken)
        {
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(command.RequestDto.EstateId, cancellationToken);

            estateAggregate.Create(command.RequestDto.EstateName);
            estateAggregate.GenerateReference();

            await this.EstateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);
        }

        public async Task AddOperatorToEstate(EstateCommands.AddOperatorToEstateCommand command, CancellationToken cancellationToken)
        {
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(command.EstateId, cancellationToken);

            estateAggregate.AddOperator(command.RequestDto.OperatorId);

            await this.EstateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);
        }

        public async Task CreateEstateUser(EstateCommands.CreateEstateUserCommand command, CancellationToken cancellationToken)
        {
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(command.EstateId, cancellationToken);

            CreateUserRequest createUserRequest = new CreateUserRequest
                                                  {
                                                      EmailAddress = command.RequestDto.EmailAddress,
                                                      FamilyName = command.RequestDto.FamilyName,
                                                      GivenName = command.RequestDto.GivenName,
                                                      MiddleName = command.RequestDto.MiddleName,
                                                      Password = command.RequestDto.Password,
                                                      PhoneNumber = "123456", // Is this really needed :|
                                                      Roles = new List<String>(),
                                                      Claims = new Dictionary<String, String>()
                                                  };

            // Check if role has been overridden
            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            createUserRequest.Roles.Add(String.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName);
            createUserRequest.Claims.Add("estateId", command.EstateId.ToString());
            
            CreateUserResponse createUserResponse = await this.SecurityServiceClient.CreateUser(createUserRequest, cancellationToken);

            // Add the user to the aggregate 
            estateAggregate.AddSecurityUser(createUserResponse.UserId, command.RequestDto.EmailAddress);

            // TODO: add a delete user here in case the aggregate add fails...

            await this.EstateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);
        }

        #endregion
    }
}