using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using Xrm.Base;
using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.WorkflowActivities.Base
{
    public abstract class BaseActivity : CodeActivity
    {
        public CodeActivityContext CodeActivityContext { get; private set; }
        public ITracingService TracingService { get; private set; }
        public IWorkflowContext WorkflowContext { get; private set; }
        public OrganizationServiceWrapper OrgServiceWrapper { get; set; }
        public ICommandBus CommandBus { get; private set; }


        protected override void Execute(CodeActivityContext executionContext)
        {
            var factory = executionContext.GetExtension<IOrganizationServiceFactory>() as IOrganizationServiceFactory;

            CodeActivityContext = executionContext;
            TracingService = executionContext.GetExtension<ITracingService>() as ITracingService;
            WorkflowContext = executionContext.GetExtension<IWorkflowContext>() as IWorkflowContext;
            IOrganizationService orgService = factory.CreateOrganizationService(WorkflowContext.UserId) as IOrganizationService;
            IOrganizationService  orgServiceAsSystem = factory.CreateOrganizationService(null) as IOrganizationService;
            OrgServiceWrapper = new OrganizationServiceWrapper(orgService, orgServiceAsSystem);
            CommandBus = new Bus(OrgServiceWrapper, TracingService);

            InternalExecute();
        }

        protected virtual void InternalExecute() { }
    }
}
