namespace EstateManagement.EstateAggregate{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Estate.DomainEvents;
    using Models.Estate;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;

    public static class EstateAggregateExtensions{
        #region Methods

        public static void AddOperator(this EstateAggregate aggregate,
                                       Guid operatorId){
            
            aggregate.CheckEstateHasBeenCreated();
            aggregate.CheckOperatorHasNotAlreadyBeenCreated(operatorId);

            OperatorAddedToEstateEvent operatorAddedToEstateEvent =
                new OperatorAddedToEstateEvent(aggregate.AggregateId, operatorId);

            aggregate.ApplyAndAppend(operatorAddedToEstateEvent);
        }

        public static void AddSecurityUser(this EstateAggregate aggregate,
                                           Guid securityUserId,
                                           String emailAddress){
            aggregate.CheckEstateHasBeenCreated();

            SecurityUserAddedToEstateEvent securityUserAddedEvent = new SecurityUserAddedToEstateEvent(aggregate.AggregateId, securityUserId, emailAddress);

            aggregate.ApplyAndAppend(securityUserAddedEvent);
        }

        public static void Create(this EstateAggregate aggregate, String estateName){
            Guard.ThrowIfNullOrEmpty(estateName, typeof(ArgumentNullException), "Estate name must be provided when registering a new estate");

            // Just return if already created
            if (aggregate.IsCreated)
                return;

            EstateCreatedEvent estateCreatedEvent = new EstateCreatedEvent(aggregate.AggregateId, estateName);

            aggregate.ApplyAndAppend(estateCreatedEvent);
        }

        public static void GenerateReference(this EstateAggregate aggregate){
            // Just return as we already have a reference allocated
            if (String.IsNullOrEmpty(aggregate.EstateReference) == false)
                return;

            aggregate.CheckEstateHasBeenCreated();

            String reference = $"{aggregate.AggregateId.GetHashCode():X}";

            EstateReferenceAllocatedEvent estateReferenceAllocatedEvent = new EstateReferenceAllocatedEvent(aggregate.AggregateId, reference);

            aggregate.ApplyAndAppend(estateReferenceAllocatedEvent);
        }

        public static Estate GetEstate(this EstateAggregate aggregate){
            Estate estateModel = new Estate();

            estateModel.EstateId = aggregate.AggregateId;
            estateModel.Name = aggregate.EstateName;
            estateModel.Reference = aggregate.EstateReference;

            if (aggregate.Operators.Any()){
                estateModel.Operators = new List<Models.Estate.Operator>();

                foreach (Operator @operator in aggregate.Operators){
                    estateModel.Operators.Add(new Models.Estate.Operator{
                                                                            OperatorId = @operator.OperatorId
                                                                        });
                }
            }

            if (aggregate.SecurityUsers.Any()){
                estateModel.SecurityUsers = new List<Models.SecurityUser>();

                foreach (SecurityUser securityUser in aggregate.SecurityUsers){
                    estateModel.SecurityUsers.Add(new Models.SecurityUser{
                                                                             EmailAddress = securityUser.EmailAddress,
                                                                             SecurityUserId = securityUser.SecurityUserId
                                                                         });
                }
            }

            return estateModel;
        }

        public static void PlayEvent(this EstateAggregate aggregate, SecurityUserAddedToEstateEvent domainEvent){
            SecurityUser securityUser = new SecurityUser(domainEvent.SecurityUserId, domainEvent.EmailAddress);

            aggregate.SecurityUsers.Add(securityUser);
        }

        public static void PlayEvent(this EstateAggregate aggregate, EstateCreatedEvent domainEvent){
            aggregate.EstateName = domainEvent.EstateName;
            aggregate.IsCreated = true;
        }

        public static void PlayEvent(this EstateAggregate aggregate, EstateReferenceAllocatedEvent domainEvent){
            aggregate.EstateReference = domainEvent.EstateReference;
        }

        /// <summary>
        /// Operators the added to estate event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        public static void PlayEvent(this EstateAggregate aggregate, OperatorAddedToEstateEvent domainEvent){
            Operator @operator = new (domainEvent.OperatorId);

            aggregate.Operators.Add(@operator);
        }

        private static void CheckEstateHasBeenCreated(this EstateAggregate aggregate){
            if (aggregate.IsCreated == false){
                throw new InvalidOperationException("Estate has not been created");
            }
        }

        private static void CheckOperatorHasNotAlreadyBeenCreated(this EstateAggregate aggregate,
                                                                  Guid operatorId){
            Operator operatorRecord = aggregate.Operators.SingleOrDefault(o => o.OperatorId == operatorId);

            if (operatorRecord != null){
                throw new InvalidOperationException($"Duplicate operator details are not allowed, an operator already exists on this estate with Id [{operatorId}]");
            }
        }

        #endregion
    }

    public record EstateAggregate : Aggregate{
        #region Fields

        internal readonly List<Operator> Operators;

        internal readonly List<SecurityUser> SecurityUsers;

        #endregion

        #region Constructors
        
        [ExcludeFromCodeCoverage]
        public EstateAggregate(){
            // Nothing here
            this.Operators = new List<Operator>();
            this.SecurityUsers = new List<SecurityUser>();
        }
        
        private EstateAggregate(Guid aggregateId){
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Operators = new List<Operator>();
            this.SecurityUsers = new List<SecurityUser>();
        }

        #endregion

        #region Properties

        public String EstateName{ get; internal set; }

        public String EstateReference{ get; internal set; }

        public Boolean IsCreated{ get; internal set; }

        #endregion

        #region Methods

        public static EstateAggregate Create(Guid aggregateId){
            return new EstateAggregate(aggregateId);
        }

        public override void PlayEvent(IDomainEvent domainEvent) => EstateAggregateExtensions.PlayEvent(this, (dynamic)domainEvent);

        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata(){
            return new{
                          EstateId = this.AggregateId
                      };
        }

        #endregion
    }
}