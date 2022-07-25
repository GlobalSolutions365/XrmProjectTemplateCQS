using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Events
{
    public class EventTriggeringCommand : IEvent
    {
        public Contact FromContact { get; set; }
    }
}
