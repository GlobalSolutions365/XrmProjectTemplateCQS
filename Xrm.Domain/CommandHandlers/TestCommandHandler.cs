using Microsoft.Xrm.Sdk;
using System;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class TestCommandHandler : CommandHandler<TestCommand, LastNameUpdated>
    {
        public TestCommandHandler(IOrganizationService orgService, IEventBus eventBus) : base(orgService, eventBus)
        {
        }

        public override LastNameUpdated Execute(TestCommand command)
        {
            // Not really best practice, but used for unit testing the command is executed
            command.IsHandled = true;

            return null;
        }
    }
}
