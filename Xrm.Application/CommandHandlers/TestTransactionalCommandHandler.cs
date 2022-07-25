using System;
using Xrm.Domain.Crm;
using Xrm.Domain.Flow;

namespace Xrm.Application.CommandHandlers
{
    public class TestTransactionalCommandHandler : CommandHandler<Commands.TestTransactionalCommand, Events.TestTransactionalEvent1>
    {
        public TestTransactionalCommandHandler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override Events.TestTransactionalEvent1 Execute(Commands.TestTransactionalCommand command)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = $"From command {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}",
                ParentCustomerId = command.TargetAccount.ToEntityReference()
            };

            OrgServiceWrapper.TransactionalOrgServiceAsSystem.Create(contact);

            return new Events.TestTransactionalEvent1 { ContactFromCommandId = contact.Id, TargetAccount = command.TargetAccount };
        }
    }
}
