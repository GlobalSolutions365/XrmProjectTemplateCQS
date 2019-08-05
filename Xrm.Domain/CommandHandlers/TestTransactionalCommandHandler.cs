using System;
using Xrm.Models.Crm;
using Xrm.Models.Flow;

namespace Xrm.Domain.CommandHandlers
{
    public class TestTransactionalCommandHandler : CommandHandler<Commands.TestTransactionalCommand, Events.TestTransactionalEvent1>
    {
        public TestTransactionalCommandHandler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override Events.TestTransactionalEvent1 Execute(Commands.TestTransactionalCommand command)
        {
            var contact = new Contact { Id = Guid.NewGuid(), FirstName = "Test", LastName = $"From command {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}" };

            orgServiceWrapper.TransactionalOrgServiceAsSystem.Create(contact);

            return new Events.TestTransactionalEvent1 { ContactFromCommandId = contact.Id };
        }
    }
}
