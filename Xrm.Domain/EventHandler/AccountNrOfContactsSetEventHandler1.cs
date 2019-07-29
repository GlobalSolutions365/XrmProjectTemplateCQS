using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class AccountNrOfContactsSetEventHandler1 : EventHandler<AccountNrOfContactsSetEvent, VoidEvent>
    {
        public AccountNrOfContactsSetEventHandler1(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override VoidEvent Execute(AccountNrOfContactsSetEvent @event)
        {
            @event.TargetContact.FirstName = "Handler1";

            return VoidEvent;
        }
    }
}
