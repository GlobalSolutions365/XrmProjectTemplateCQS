using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using Xrm.Infrastructure;
using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;
using DateProvider;

namespace Xrm.WorkflowActivities.Base
{
    public abstract class BaseActivity : CodeActivity
    {
        private Bus bus = new Bus();

        public CodeActivityContext CodeActivityContext { get; private set; }
        public ITracingService TracingService { get; private set; }
        public IWorkflowContext WorkflowContext { get; private set; }
        public OrganizationServiceWrapper OrgServiceWrapper { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var factory = executionContext.GetExtension<IOrganizationServiceFactory>() as IOrganizationServiceFactory;

            CodeActivityContext = executionContext;
            TracingService = executionContext.GetExtension<ITracingService>() as ITracingService;
            WorkflowContext = executionContext.GetExtension<IWorkflowContext>() as IWorkflowContext;
            IOrganizationService orgService = factory.CreateOrganizationService(WorkflowContext.UserId) as IOrganizationService;
            IOrganizationService orgServiceAsSystem = factory.CreateOrganizationService(null) as IOrganizationService;
            OrgServiceWrapper = new OrganizationServiceWrapper(orgService, orgServiceAsSystem, new TransactionalService(orgService), new TransactionalService(orgServiceAsSystem));       

            InternalExecute();
        }

        protected virtual void InternalExecute() { }

        protected void Handle(ICommand command)
        {
            bus.Handle(command, new Domain.Flow.FlowArguments(OrgServiceWrapper, TracingService, bus, bus));
        }
    }
}
