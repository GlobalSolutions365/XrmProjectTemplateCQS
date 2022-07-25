using Microsoft.Xrm.Sdk;
using Xrm.Application.Commands;
using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.CommandHandlers
{
    public class TestCommandHandler : CommandHandler<TestCommand, TestCommandExecutedEvent>
    {
        public TestCommandHandler(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override TestCommandExecutedEvent Execute(TestCommand command)
        {
            // Not really best practice, but used for unit testing the command is executed
            command.IsHandled = true;

            return new TestCommandExecutedEvent();
        }
    }
}
