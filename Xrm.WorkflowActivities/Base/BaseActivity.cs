using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace Xrm.WorkflowActivities.Base
{
    public class BaseActivity : CodeActivity
    {
        public CodeActivityContext CodeActivityContext { get; private set; }
        public ITracingService TracingService { get; private set; }
        public IWorkflowContext WorkflowContext { get; private set; }
        public IOrganizationService OrgService { get; private set; }
        public IOrganizationService OrgServiceAsSystem { get; private set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var factory = executionContext.GetExtension<IOrganizationServiceFactory>() as IOrganizationServiceFactory;

            CodeActivityContext = executionContext;
            TracingService = executionContext.GetExtension<ITracingService>() as ITracingService;
            WorkflowContext = executionContext.GetExtension<IWorkflowContext>() as IWorkflowContext;
            OrgService = factory.CreateOrganizationService(WorkflowContext.UserId) as IOrganizationService;
            OrgServiceAsSystem = factory.CreateOrganizationService(null) as IOrganizationService;

            InternalExecute();
        }

        protected virtual void InternalExecute() { }
    }
}
