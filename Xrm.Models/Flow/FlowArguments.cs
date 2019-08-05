using System;
using Xrm.Models.Interfaces;

namespace Xrm.Models.Flow
{
    public class FlowArguments
    {
        public FlowArguments(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus)
        {
            this.OrgServiceWrapper = orgServiceWrapper ?? throw new ArgumentNullException(nameof(orgServiceWrapper));
            this.EventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public IOrganizationServiceWrapper OrgServiceWrapper { get; }
        public IEventBus EventBus { get; }
    }
}
