using Microsoft.Xrm.Sdk;
using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.EventHandler
{
    public class TestEventHandler1 : EventHandler<TestEvent, VoidEvent>
    {
        public TestEventHandler1(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(TestEvent @event)
        {
            @event.IsHandled1 = true;

            return null;
        }
    }
}
