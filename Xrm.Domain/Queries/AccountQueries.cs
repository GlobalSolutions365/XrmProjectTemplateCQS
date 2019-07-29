using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using Xrm.Models.Crm;

namespace Xrm.Domain.Queries
{
    public class AccountQueries : CrmQuery<Account>
    {
        public AccountQueries(IOrganizationService orgService) : base(orgService) { }

        public EntityReference[] GetChildContactReferences(Guid accountId)
        {
            using(XrmContext xrm = new XrmContext(OrgService))
            {
                return xrm.ContactSet
                          .Where(c => c.AccountId != null && c.AccountId.Id == accountId)
                          .Select(c => c.ToEntityReference())
                          .ToArray();
            }
        }

        public string GetName(Guid accountId)
        {
            using (XrmContext xrm = new XrmContext(OrgService))
            {
                return xrm.AccountSet
                          .Where(a => a.AccountId == accountId)
                          .Select(a => a.Name)
                          .FirstOrDefault();
            }
        }

        public int GetNrOfContacts(Guid accountId)
        {
            using (XrmContext xrm = new XrmContext(OrgService))
            {
                return xrm.ContactSet
                          .Where(c => c.ParentCustomerId != null && c.ParentCustomerId.Id == accountId)
                          .Select(c => c.Id)
                          .ToArray()
                          .Length;
            }
        }
    }
}
