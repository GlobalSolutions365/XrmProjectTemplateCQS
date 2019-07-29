using Microsoft.Xrm.Sdk;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class TestEventHandler2 : EventHandler<TestEvent, VoidEvent>
    {
        public TestEventHandler2(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override VoidEvent Execute(TestEvent @event)
        {
            @event.IsHandled2 = true;

            return null;
        }
    }    
}
