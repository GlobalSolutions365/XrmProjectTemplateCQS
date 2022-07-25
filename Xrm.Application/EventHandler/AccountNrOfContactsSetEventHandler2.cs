using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.EventHandler
{
    public class AccountNrOfContactsSetEventHandler2 : EventHandler<AccountNrOfContactsSetEvent, LastNameChangedEvent>
    {
        public AccountNrOfContactsSetEventHandler2(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override LastNameChangedEvent Execute(AccountNrOfContactsSetEvent @event)
        {
            @event.TargetContact.LastName = "Handler2";

            return new LastNameChangedEvent { TargetContact = @event.TargetContact };
        }
    }
}
