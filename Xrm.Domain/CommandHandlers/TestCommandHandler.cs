using Microsoft.Xrm.Sdk;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class TestCommandHandler : CommandHandler<TestCommand, TestCommandExecuted>
    {
        public TestCommandHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override TestCommandExecuted Execute(TestCommand command)
        {
            // Not really best practice, but used for unit testing the command is executed
            command.IsHandled = true;

            return new TestCommandExecuted();
        }
    }
}
