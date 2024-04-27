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

        public static void RemoveOperator(this EstateAggregate aggregate,
                                       Guid operatorId)
        {

            aggregate.CheckEstateHasBeenCreated();
            aggregate.CheckOperatorHasBeenAdded(operatorId);

            OperatorRemovedFromEstateEvent operatorRemovedFromEstateEvent =
                new OperatorRemovedFromEstateEvent(aggregate.AggregateId, operatorId);

            aggregate.ApplyAndAppend(operatorRemovedFromEstateEvent);
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

                foreach (KeyValuePair<Guid, Operator> @operator in aggregate.Operators){
                    estateModel.Operators.Add(new Models.Estate.Operator{
                                                                            OperatorId = @operator.Key,
                                                                            IsDeleted = @operator.Value.IsDeleted,
                                                                        });
                }
            }

            if (aggregate.SecurityUsers.Any()){
                estateModel.SecurityUsers = new List<Models.SecurityUser>();

                foreach (KeyValuePair<Guid, SecurityUser> securityUser in aggregate.SecurityUsers){
                    estateModel.SecurityUsers.Add(new Models.SecurityUser{
                                                                             EmailAddress = securityUser.Value.EmailAddress,
                                                                             SecurityUserId = securityUser.Key
                                                                         });
                }
            }

            return estateModel;
        }

        public static void PlayEvent(this EstateAggregate aggregate, SecurityUserAddedToEstateEvent domainEvent){
            SecurityUser securityUser = new (domainEvent.EmailAddress);

            aggregate.SecurityUsers.Add(domainEvent.SecurityUserId,securityUser);
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
            Operator @operator = new ();

            aggregate.Operators.Add(domainEvent.OperatorId, @operator);
        }

        public static void PlayEvent(this EstateAggregate aggregate, OperatorRemovedFromEstateEvent domainEvent){
            KeyValuePair<Guid, Operator> @operator = aggregate.Operators.Single(o => o.Key == domainEvent.OperatorId);
            aggregate.Operators[domainEvent.OperatorId] = @operator.Value with{
                                                                                  IsDeleted = true
                                                                              };
        }

        private static void CheckEstateHasBeenCreated(this EstateAggregate aggregate){
            if (aggregate.IsCreated == false){
                throw new InvalidOperationException("Estate has not been created");
            }
        }

        private static void CheckOperatorHasNotAlreadyBeenCreated(this EstateAggregate aggregate,
                                                                  Guid operatorId){
            Boolean operatorRecord = aggregate.Operators.ContainsKey(operatorId);

            if (operatorRecord == true){
                throw new InvalidOperationException($"Duplicate operator details are not allowed, an operator already exists on this estate with Id [{operatorId}]");
            }
        }

        private static void CheckOperatorHasBeenAdded(this EstateAggregate aggregate,
                                                      Guid operatorId)
        {
            Boolean operatorRecord = aggregate.Operators.ContainsKey(operatorId);

            if (operatorRecord == false)
            {
                throw new InvalidOperationException($"Operator not added to this Estate with Id [{operatorId}]");
            }
        }

        #endregion
    }

    public record EstateAggregate : Aggregate{
        #region Fields

        internal readonly Dictionary<Guid, Operator> Operators;

        internal readonly Dictionary<Guid, SecurityUser> SecurityUsers;

        #endregion

        #region Constructors
        
        [ExcludeFromCodeCoverage]
        public EstateAggregate(){
            // Nothing here
            this.Operators = new Dictionary<Guid, Operator>();
            this.SecurityUsers = new Dictionary<Guid, SecurityUser>();
        }
        
        private EstateAggregate(Guid aggregateId){
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Operators = new Dictionary<Guid, Operator>();
            this.SecurityUsers = new Dictionary<Guid, SecurityUser>();
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