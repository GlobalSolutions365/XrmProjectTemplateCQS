using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Events
{
    public class AccountNrOfContactsSetEvent : IEvent
    {
        public Contact TargetContact { get; set; }
    }
}
