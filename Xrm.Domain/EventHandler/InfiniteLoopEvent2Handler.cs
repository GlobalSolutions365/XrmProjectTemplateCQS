using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.EventHandler
{
    public class InfiniteLoopEvent2Handler : EventHandler<InfiniteLoopEvent2, InfiniteLoopEvent1>
    {
        public InfiniteLoopEvent2Handler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override InfiniteLoopEvent1 Execute(InfiniteLoopEvent2 @event)
        {
            return new InfiniteLoopEvent1();
        }
    }
}
