using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class SampleEventHandler : EventHandler<SampleEvent, VoidEvent>
    {
        public SampleEventHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override VoidEvent Execute(SampleEvent @event)
        {
            return VoidEvent;
        }
    }
}
