using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class AccountNrOfContactsSetEventHandler2 : EventHandler<AccountNrOfContactsSetEvent, LastNameChangedEvent>
    {
        public AccountNrOfContactsSetEventHandler2(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override LastNameChangedEvent Execute(AccountNrOfContactsSetEvent @event)
        {
            @event.TargetContact.LastName = "Handler2";

            return new LastNameChangedEvent { TargetContact = @event.TargetContact };
        }
    }
}
