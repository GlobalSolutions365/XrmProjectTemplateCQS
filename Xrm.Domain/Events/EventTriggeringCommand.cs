using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Events
{
    public class EventTriggeringCommand : IEvent
    {
        public Contact FromContact { get; set; }
    }
}
