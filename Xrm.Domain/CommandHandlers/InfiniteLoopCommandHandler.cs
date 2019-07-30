using System;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class InfiniteLoopCommandHandler : CommandHandler<InfiniteLoopCommand, InfiniteLoopEvent1>
    {
        public InfiniteLoopCommandHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override InfiniteLoopEvent1 Execute(InfiniteLoopCommand command)
        {
            return new InfiniteLoopEvent1();
        }
    }
}
