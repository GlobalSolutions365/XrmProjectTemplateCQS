using Microsoft.Xrm.Sdk;
using System;
using Xrm.Domain.Interfaces;

namespace Xrm.Domain.Crm
{
    public class OrganizationServiceWrapper : IOrganizationServiceWrapper
    {
        public IOrganizationService OrgService { get; }
        public IOrganizationService OrgServiceAsSystem { get; }   
        public IOrganizationService TransactionalOrgService { get; }
        public IOrganizationService TransactionalOrgServiceAsSystem { get; }

        public OrganizationServiceWrapper(IOrganizationService orgService) : this(orgService, orgService)
        {
        }

        public OrganizationServiceWrapper(IOrganizationService orgService, IOrganizationService orgServiceAsSystem) : this(orgService, orgServiceAsSystem, orgService, orgServiceAsSystem)
        {
        }

        public OrganizationServiceWrapper(IOrganizationService orgService, IOrganizationService orgServiceAsSystem, IOrganizationService transactionalOrgService, IOrganizationService transactionalOrgServiceAsSystem)
        {
            this.OrgService = orgService ?? throw new ArgumentNullException(nameof(orgService));
            this.OrgServiceAsSystem = orgServiceAsSystem ?? throw new ArgumentNullException(nameof(orgServiceAsSystem));
            this.TransactionalOrgService = transactionalOrgService ?? throw new ArgumentNullException(nameof(transactionalOrgService));
            this.TransactionalOrgServiceAsSystem = transactionalOrgServiceAsSystem ?? throw new ArgumentNullException(nameof(transactionalOrgServiceAsSystem));
        }
    }
}
