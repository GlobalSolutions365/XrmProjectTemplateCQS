using Xrm.Domain.Events;
using Xrm.Models.Flow;

namespace Xrm.Domain.EventHandler
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
