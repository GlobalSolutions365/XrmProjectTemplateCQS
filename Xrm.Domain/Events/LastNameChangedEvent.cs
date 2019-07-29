using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Events
{
    public class LastNameChangedEvent : IEvent
    {
        public Contact TargetContact { get; set; }
    }
}
