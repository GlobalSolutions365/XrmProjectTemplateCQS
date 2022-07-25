using System;
using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.EventHandler
{
    public class TestCommandExecutedEventHandler : EventHandler<TestCommandExecutedEvent, VoidEvent>
    {
        public TestCommandExecutedEventHandler(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(TestCommandExecutedEvent @event)
        {
            TestCommandExecutedEvent.IsHandled = true;

            return VoidEvent;
        }
    }
}
