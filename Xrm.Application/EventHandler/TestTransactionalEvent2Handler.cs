using System;
using Xrm.Domain.Crm;
using Xrm.Domain.Flow;

namespace Xrm.Application.EventHandler
{
    public class TestTransactionalEvent2Handler : EventHandler<Events.TestTransactionalEvent2, Events.VoidEvent>
    {
        public TestTransactionalEvent2Handler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override Events.VoidEvent Execute(Events.TestTransactionalEvent2 @event)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = $"From event2 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}",
                ParentCustomerId = @event.TargetAccount.ToEntityReference()
            };

            OrgServiceWrapper.TransactionalOrgServiceAsSystem.Create(contact);

            return VoidEvent;
        }
    }
}
