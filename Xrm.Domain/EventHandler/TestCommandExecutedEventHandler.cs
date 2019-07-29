using System;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class TestCommandExecutedEventHandler : EventHandler<TestCommandExecutedEvent, VoidEvent>
    {
        public TestCommandExecutedEventHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override VoidEvent Execute(TestCommandExecutedEvent @event)
        {
            TestCommandExecutedEvent.IsHandled = true;

            return VoidEvent;
        }
    }
}
