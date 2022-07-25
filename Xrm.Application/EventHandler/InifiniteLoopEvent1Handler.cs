using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.EventHandler
{
    public class InifiniteLoopEvent1Handler : EventHandler<InfiniteLoopEvent1, InfiniteLoopEvent2>
    {
        public InifiniteLoopEvent1Handler(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override InfiniteLoopEvent2 Execute(InfiniteLoopEvent1 @event)
        {
            return new InfiniteLoopEvent2();
        }
    }
}
