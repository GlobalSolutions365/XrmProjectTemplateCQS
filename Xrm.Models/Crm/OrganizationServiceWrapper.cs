using Microsoft.Xrm.Sdk;
using System;
using Xrm.Models.Interfaces;

namespace Xrm.Models.Crm
{
    public class OrganizationServiceWrapper : IOrganizationServiceWrapper
    {
        public IOrganizationService OrgService { get; }
        public IOrganizationService OrgServiceAsSystem { get; }

        public OrganizationServiceWrapper(IOrganizationService orgService) : this(orgService, orgService)
        {
        }

        public OrganizationServiceWrapper(IOrganizationService orgService, IOrganizationService orgServiceAsSystem)
        {
            this.OrgService = orgService ?? throw new ArgumentNullException(nameof(orgService));
            this.OrgServiceAsSystem = orgServiceAsSystem ?? throw new ArgumentNullException(nameof(orgServiceAsSystem));
        }
    }
}
