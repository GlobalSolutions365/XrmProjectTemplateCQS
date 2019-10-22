using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Xrm.Infrastructure;
using Xrm.Models.Crm;
using Xrm.Models.Flow;
using Xrm.Models.Interfaces;
using Xrm.UnitTests.Fakes;

namespace Xrm.UnitTests
{
    public class BaseCrmTest
    {
        protected XrmFakedContext Context { get; set; } = new XrmFakedContext();

        protected IOrganizationService OrgService => Context.GetOrganizationService();

        protected OrganizationServiceWrapper OrgServiceWrapper => new OrganizationServiceWrapper(OrgService);

        protected ITracingService FakeTracing = new FakeTracingService();

        protected ICommandBus CmdBus => new Bus();

        protected ICommandBus CmdBusWithNoEventPropagation => new Bus() { DoNotPropagateEvents = true };

        protected IEventBus EventBus => new Bus();

        protected FlowArguments FlowArgs => new FlowArguments(OrgServiceWrapper, FakeTracing, EventBus, CmdBus);

    }
}
