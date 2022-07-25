using System;
using Xrm.Application.Commands;
using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.CommandHandlers
{
    public class InfiniteLoopCommandHandler : CommandHandler<InfiniteLoopCommand, InfiniteLoopEvent1>
    {
        public InfiniteLoopCommandHandler(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override InfiniteLoopEvent1 Execute(InfiniteLoopCommand command)
        {
            return new InfiniteLoopEvent1();
        }
    }
}
