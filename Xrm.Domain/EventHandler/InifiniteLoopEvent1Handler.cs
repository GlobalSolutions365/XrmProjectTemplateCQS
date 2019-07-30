using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class InifiniteLoopEvent1Handler : EventHandler<InfiniteLoopEvent1, InfiniteLoopEvent2>
    {
        public InifiniteLoopEvent1Handler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override InfiniteLoopEvent2 Execute(InfiniteLoopEvent1 @event)
        {
            return new InfiniteLoopEvent2();
        }
    }
}
