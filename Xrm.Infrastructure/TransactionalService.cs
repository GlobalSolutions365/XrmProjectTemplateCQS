using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace Xrm.Infrastructure
{
    public class TransactionalService : IOrganizationService
    {
        private readonly IOrganizationService orgService;
        private readonly List<OrganizationRequest> requests = new List<OrganizationRequest>();

        public TransactionalService(IOrganizationService orgService)
        {
            this.orgService = orgService ?? throw new ArgumentNullException(nameof(orgService));
        }

        public void Commit()
        {
            if (requests.Count == 0)
            {
                return;
            }

            var executeTransactionRequest = new ExecuteTransactionRequest
            {
                Requests = new OrganizationRequestCollection(),
                ReturnResponses = false
            };

            executeTransactionRequest.Requests.AddRange(requests);

            orgService.Execute(executeTransactionRequest);
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            requests.Add(
                new AssociateRequest() { Target = new EntityReference(entityName, entityId), Relationship = relationship, RelatedEntities = relatedEntities }
            );
        }

        public Guid Create(Entity entity)
        {
            requests.Add(new CreateRequest() { Target = entity });
            return Guid.Empty;
        }

        public void Delete(string entityName, Guid id)
        {
            requests.Add(new DeleteRequest() { Target = new EntityReference(entityName, id) });
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            requests.Add(
                new DisassociateRequest() { Target = new EntityReference(entityName, entityId), Relationship = relationship, RelatedEntities = relatedEntities }
            );
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            requests.Add(request);
            return new OrganizationResponse();
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            throw new NotImplementedException();
        }

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            throw new NotImplementedException();
        }

        public void Update(Entity entity)
        {
            requests.Add(new UpdateRequest() { Target = entity });
        }
    }
}
