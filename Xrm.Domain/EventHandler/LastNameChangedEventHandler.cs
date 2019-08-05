using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class LastNameChangedEventHandler : EventHandler<LastNameChangedEvent, VoidEvent>
    {
        public LastNameChangedEventHandler(Models.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(LastNameChangedEvent @event)
        {
            @event.TargetContact.EMailAddress1 = "test@test.com";

            return VoidEvent;
        }
    }
}
