using Xrm.Application.Events;

namespace Xrm.Application.EventHandler
{
    public class AccountNrOfContactsSetEventHandler1 : EventHandler<AccountNrOfContactsSetEvent, VoidEvent>
    {
        public AccountNrOfContactsSetEventHandler1(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(AccountNrOfContactsSetEvent @event)
        {
            @event.TargetContact.FirstName = "Handler1";

            return VoidEvent;
        }
    }
}
