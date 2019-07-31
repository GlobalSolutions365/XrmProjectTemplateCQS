using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Xrm.Base;
using Xrm.Models.Crm;
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

        protected ICommandBus CmdBus => new Bus(OrgServiceWrapper, FakeTracing);

        protected ICommandBus CmdBusWithNoEventPropagation => new Bus(OrgServiceWrapper, FakeTracing) { DoNotPropagateEvents = true };

        protected IEventBus EventBus => new Bus(OrgServiceWrapper, FakeTracing);

    }
}
