using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.EventHandler
{
    public class LastNameChangedEventHandler : EventHandler<LastNameChangedEvent, VoidEvent>
    {
        public LastNameChangedEventHandler(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(LastNameChangedEvent @event)
        {
            @event.TargetContact.EMailAddress1 = "test@test.com";

            return VoidEvent;
        }
    }
}
