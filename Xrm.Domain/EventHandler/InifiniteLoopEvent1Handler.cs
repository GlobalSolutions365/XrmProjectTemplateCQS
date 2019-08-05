using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class InifiniteLoopEvent1Handler : EventHandler<InfiniteLoopEvent1, InfiniteLoopEvent2>
    {
        public InifiniteLoopEvent1Handler(Models.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override InfiniteLoopEvent2 Execute(InfiniteLoopEvent1 @event)
        {
            return new InfiniteLoopEvent2();
        }
    }
}
