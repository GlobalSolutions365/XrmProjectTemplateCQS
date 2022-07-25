using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using Xrm.Application.Commands;
using Xrm.Domain.Crm;

namespace Xrm.WorkflowActivities
{
    public class SampleWorkflowActivity : Base.BaseActivity
    {
        [RequiredArgument]
        [ReferenceTarget(Contact.EntityLogicalName)]
        [Input("Contact")]
        public InArgument<EntityReference> ContactRef { get; set; }

        protected override void InternalExecute()
        {
            EntityReference contactRef = ContactRef.Get(CodeActivityContext);

            var cmd = new TestCommand
            {
                IsHandled = false,
                SomeId = contactRef.Id
            };

            Handle(cmd);
        }
    }
}
