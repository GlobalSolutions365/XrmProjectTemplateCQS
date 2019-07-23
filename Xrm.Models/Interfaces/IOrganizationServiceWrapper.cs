using Microsoft.Xrm.Sdk;

namespace Xrm.Models.Interfaces
{
    public interface IOrganizationServiceWrapper
    {
        IOrganizationService OrgService { get; }
        IOrganizationService OrgServiceAsSystem { get; }
    }
}
