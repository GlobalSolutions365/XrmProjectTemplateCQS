using Xrm.Domain.Events;
using Xrm.Models.Flow;

namespace Xrm.Domain.EventHandler
{
    public class BranchedEvent1Handler : EventHandler<BranchedEvent1, VoidEvent>
    {
        public BranchedEvent1Handler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(BranchedEvent1 @event)
        {
            TracingService.Trace(nameof(BranchedEvent1Handler));

            return VoidEvent;
        }
    }
}
