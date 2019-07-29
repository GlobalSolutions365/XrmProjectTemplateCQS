using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Events
{
    public class AccountNrOfContactsSetEvent : IEvent
    {
        public Contact TargetContact { get; set; }
    }
}
