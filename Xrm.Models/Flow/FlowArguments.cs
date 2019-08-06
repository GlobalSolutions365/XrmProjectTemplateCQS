using Microsoft.Xrm.Sdk;
using System;
using Xrm.Models.Interfaces;

namespace Xrm.Models.Flow
{
    public class FlowArguments
    {
        public IOrganizationServiceWrapper OrgServiceWrapper { get; }
        public IEventBus EventBus { get; }
        public ITracingService TracingService { get; }

        public FlowArguments(IOrganizationServiceWrapper orgServiceWrapper, ITracingService tracingService, IEventBus eventBus)
        {
            this.OrgServiceWrapper = orgServiceWrapper ?? throw new ArgumentNullException(nameof(orgServiceWrapper));
            this.EventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.TracingService = tracingService ?? throw new ArgumentNullException(nameof(tracingService));
        }
    }
}
