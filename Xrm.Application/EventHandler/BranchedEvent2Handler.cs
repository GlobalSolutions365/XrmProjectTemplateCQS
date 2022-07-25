using Xrm.Application.Events;
using Xrm.Domain.Flow;

namespace Xrm.Application.EventHandler
{
    public class BranchedEvent2Handler : EventHandler<BranchedEvent2, VoidEvent>
    {
        public BranchedEvent2Handler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(BranchedEvent2 @event)
        {
            TracingService.Trace(nameof(BranchedEvent2Handler));

            return VoidEvent;
        }
    }
}
