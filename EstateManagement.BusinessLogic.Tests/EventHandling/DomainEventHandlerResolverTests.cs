namespace EstateManagement.BusinessLogic.Tests.EventHandling;

using System;
using System.Collections.Generic;
using System.Linq;
using Estate.DomainEvents;
using Moq;
using Shared.EventStore.EventHandling;
using Shouldly;
using Testing;
using Xunit;

public class DomainEventHandlerResolverTests
{
    [Fact]
    public void DomainEventHandlerResolver_CanBeCreated_IsCreated()
    {
        Dictionary<String, String[]> eventHandlerConfiguration = new Dictionary<String, String[]>();

        eventHandlerConfiguration.Add("TestEventType1", new String[] { "EstateManagement.BusinessLogic.EventHandling.EstateDomainEventHandler, EstateManagement.BusinessLogic" });

        Mock<IDomainEventHandler> domainEventHandler = new Mock<IDomainEventHandler>();
        Func<Type, IDomainEventHandler> createDomainEventHandlerFunc = (type) => { return domainEventHandler.Object; };
        DomainEventHandlerResolver resolver = new DomainEventHandlerResolver(eventHandlerConfiguration, createDomainEventHandlerFunc);

        resolver.ShouldNotBeNull();
    }

    [Fact]
    public void DomainEventHandlerResolver_CanBeCreated_InvalidEventHandlerType_ErrorThrown()
    {
        Dictionary<String, String[]> eventHandlerConfiguration = new Dictionary<String, String[]>();

        eventHandlerConfiguration.Add("TestEventType1", new String[] { "EstateManagement.BusinessLogic.EventHandling.NonExistantDomainEventHandler, EstateManagement.BusinessLogic" });

        Mock<IDomainEventHandler> domainEventHandler = new Mock<IDomainEventHandler>();
        Func<Type, IDomainEventHandler> createDomainEventHandlerFunc = (type) => { return domainEventHandler.Object; };

        Should.Throw<NotSupportedException>(() => new DomainEventHandlerResolver(eventHandlerConfiguration, createDomainEventHandlerFunc));
    }

    [Fact]
    public void DomainEventHandlerResolver_GetDomainEventHandlers_EstateCreatedEvent_EventHandlersReturned()
    {
        String handlerTypeName = "EstateManagement.BusinessLogic.EventHandling.EstateDomainEventHandler, EstateManagement.BusinessLogic";
        Dictionary<String, String[]> eventHandlerConfiguration = new Dictionary<String, String[]>();

        EstateCreatedEvent estateCreatedEvent = TestData.EstateCreatedEvent;

        eventHandlerConfiguration.Add(estateCreatedEvent.GetType().Name, new String[] { handlerTypeName });

        Mock<IDomainEventHandler> domainEventHandler = new Mock<IDomainEventHandler>();
        Func<Type, IDomainEventHandler> createDomainEventHandlerFunc = (type) => { return domainEventHandler.Object; };

        DomainEventHandlerResolver resolver = new DomainEventHandlerResolver(eventHandlerConfiguration, createDomainEventHandlerFunc);

        List<IDomainEventHandler> handlers = resolver.GetDomainEventHandlers(estateCreatedEvent);

        handlers.ShouldNotBeNull();
        handlers.Any().ShouldBeTrue();
        handlers.Count.ShouldBe(1);
    }

    [Fact]
    public void DomainEventHandlerResolver_GetDomainEventHandlers_EstateCreatedEvent_EventNotConfigured_EventHandlersReturned()
    {
        String handlerTypeName = "EstateManagement.BusinessLogic.EventHandling.EstateDomainEventHandler, EstateManagement.BusinessLogic";
        Dictionary<String, String[]> eventHandlerConfiguration = new Dictionary<String, String[]>();

        EstateCreatedEvent estateCreatedEvent = TestData.EstateCreatedEvent;

        eventHandlerConfiguration.Add("RandomEvent", new String[] { handlerTypeName });
        Mock<IDomainEventHandler> domainEventHandler = new Mock<IDomainEventHandler>();
        Func<Type, IDomainEventHandler> createDomainEventHandlerFunc = (type) => { return domainEventHandler.Object; };

        DomainEventHandlerResolver resolver = new DomainEventHandlerResolver(eventHandlerConfiguration, createDomainEventHandlerFunc);

        List<IDomainEventHandler> handlers = resolver.GetDomainEventHandlers(estateCreatedEvent);

        handlers.ShouldBeNull();
    }

    [Fact]
    public void DomainEventHandlerResolver_GetDomainEventHandlers_EstateCreatedEvent_NoHandlersConfigured_EventHandlersReturned()
    {
        Dictionary<String, String[]> eventHandlerConfiguration = new Dictionary<String, String[]>();

        EstateCreatedEvent estateCreatedEvent = TestData.EstateCreatedEvent;
        Mock<IDomainEventHandler> domainEventHandler = new Mock<IDomainEventHandler>();

        Func<Type, IDomainEventHandler> createDomainEventHandlerFunc = (type) => { return domainEventHandler.Object; };

        DomainEventHandlerResolver resolver = new DomainEventHandlerResolver(eventHandlerConfiguration, createDomainEventHandlerFunc);

        List<IDomainEventHandler> handlers = resolver.GetDomainEventHandlers(estateCreatedEvent);

        handlers.ShouldBeNull();
    }
}