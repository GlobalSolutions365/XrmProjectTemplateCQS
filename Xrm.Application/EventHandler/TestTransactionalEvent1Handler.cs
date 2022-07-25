using Xrm.Domain.Crm;
using Xrm.Domain.Flow;

namespace Xrm.Application.EventHandler
{
    public class TestTransactionalEvent1Handler : EventHandler<Events.TestTransactionalEvent1, Events.TestTransactionalEvent2>
    {
        public TestTransactionalEvent1Handler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override Events.TestTransactionalEvent2 Execute(Events.TestTransactionalEvent1 @event)
        {
            var contactToUpdate = new Contact
            {
                Id = @event.ContactFromCommandId,
                FirstName = "Test event"
            };

            OrgServiceWrapper.TransactionalOrgServiceAsSystem.Update(contactToUpdate);

            return new Events.TestTransactionalEvent2 { TargetAccount = @event .TargetAccount };
        }
    }
}
