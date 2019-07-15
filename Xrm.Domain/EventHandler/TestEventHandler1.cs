using Microsoft.Xrm.Sdk;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class TestEventHandler1 : EventHandler<TestEvent, VoidEvent>
    {
        public TestEventHandler1(IOrganizationService orgService, IEventBus eventBus) : base(orgService, eventBus)
        {
        }

        public override VoidEvent Execute(TestEvent @event)
        {
            @event.IsHandled1 = true;

            return null;
        }
    }
}
