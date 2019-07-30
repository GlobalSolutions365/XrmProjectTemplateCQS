using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Xrm.Base;
using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.UnitTests
{
    public class BaseCrmTest
    {
        protected XrmFakedContext Context { get; set; } = new XrmFakedContext();

        protected IOrganizationService OrgService => Context.GetOrganizationService();

        protected OrganizationServiceWrapper OrgServiceWrapper => new OrganizationServiceWrapper(OrgService);

        protected ICommandBus CmdBus => new Bus(OrgServiceWrapper);

        protected IEventBus EventBus => new Bus(OrgServiceWrapper);

    }
}
