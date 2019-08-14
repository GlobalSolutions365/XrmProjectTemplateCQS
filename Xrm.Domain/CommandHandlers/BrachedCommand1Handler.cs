using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Flow;

namespace Xrm.Domain.CommandHandlers
{
    public class BrachedCommand1Handler : CommandHandler<BranchedCommand1, BranchedCommand1HandledEvent>
    {
        public BrachedCommand1Handler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override BranchedCommand1HandledEvent Execute(BranchedCommand1 command)
        {
            TracingService.Trace(nameof(BrachedCommand1Handler));

            if (command.ExecuteBranch1)
            {
                return new BranchedEvent1();
            }
            else
            {
                return new BranchedEvent2();
            }
        }
    }
}
