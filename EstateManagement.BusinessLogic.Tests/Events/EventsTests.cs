using System;

namespace EstateManagement.BusinessLogic.Tests.Events
{
    using EstateManagement.BusinessLogic.Events;
    using Shouldly;
    using Testing;
    using Xunit;

    public class EventsTests
    {
        [Fact]
        public void CallbackReceivedEnrichedEvent_CanBeCreated_IsCreated(){
            CallbackReceivedEnrichedEvent callbackReceivedEnrichedEvent = new CallbackReceivedEnrichedEvent(TestData.CallbackId,
                                                                                                            TestData.EstateId,
                                                                                                            TestData.CallbackMessageFormat,
                                                                                                            TestData.CallbackReference,
                                                                                                            TestData.CallbackTypeString,
                                                                                                            TestData.CallbackMessage);
            callbackReceivedEnrichedEvent.ShouldNotBeNull();
            callbackReceivedEnrichedEvent.EventId.ShouldNotBe(Guid.Empty);
            callbackReceivedEnrichedEvent.AggregateId.ShouldBe(TestData.CallbackId);
            callbackReceivedEnrichedEvent.EstateId.ShouldBe(TestData.EstateId);
            callbackReceivedEnrichedEvent.MessageFormat.ShouldBe(TestData.CallbackMessageFormat);
            callbackReceivedEnrichedEvent.Reference.ShouldBe(TestData.CallbackReference);
            callbackReceivedEnrichedEvent.TypeString.ShouldBe(TestData.CallbackTypeString);
            callbackReceivedEnrichedEvent.CallbackMessage.ShouldBe(TestData.CallbackMessage);

        }
    }
}
