using Microsoft.Xrm.Sdk;
using System;
using Xrm.Domain.Interfaces;

namespace Xrm.Domain.Flow
{
    public class FlowArguments
    {
        public IOrganizationServiceWrapper OrgServiceWrapper { get; }
        public ICommandBus CommandBus { get; }
        public IEventBus EventBus { get; }
        public ITracingService TracingService { get; }        

        public FlowArguments(IOrganizationServiceWrapper orgServiceWrapper, ITracingService tracingService, IEventBus eventBus, ICommandBus commandBus)
        {
            this.OrgServiceWrapper = orgServiceWrapper ?? throw new ArgumentNullException(nameof(orgServiceWrapper));
            this.EventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.CommandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
            this.TracingService = tracingService ?? throw new ArgumentNullException(nameof(tracingService));
        }
    }
}
