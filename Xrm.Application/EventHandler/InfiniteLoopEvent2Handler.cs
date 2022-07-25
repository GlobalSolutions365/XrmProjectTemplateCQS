using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.EventHandler
{
    public class InfiniteLoopEvent2Handler : EventHandler<InfiniteLoopEvent2, InfiniteLoopEvent1>
    {
        public InfiniteLoopEvent2Handler(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override InfiniteLoopEvent1 Execute(InfiniteLoopEvent2 @event)
        {
            return new InfiniteLoopEvent1();
        }
    }
}
