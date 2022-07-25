using Microsoft.Xrm.Sdk;

namespace Xrm.Domain.Interfaces
{
    public interface IOrganizationServiceWrapper
    {
        IOrganizationService OrgService { get; }
        IOrganizationService OrgServiceAsSystem { get; }
        IOrganizationService TransactionalOrgService { get; }
        IOrganizationService TransactionalOrgServiceAsSystem { get; }
    }
}
