using Microsoft.Xrm.Sdk;
using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.EventHandler
{
    public class TestEventHandler2 : EventHandler<TestEvent, VoidEvent>
    {
        public TestEventHandler2(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(TestEvent @event)
        {
            @event.IsHandled2 = true;

            return null;
        }
    }    
}
