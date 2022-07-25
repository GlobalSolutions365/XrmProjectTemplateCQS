using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using Xrm.Domain.Interfaces;

namespace Xrm.Application
{
    public abstract class CrmQuery<TEntity> where TEntity : Entity
    {
        protected IOrganizationService OrgService { get; }
        protected string EntityName { get; }

        public CrmQuery(IOrganizationService orgService)
        {
            this.OrgService = orgService ?? throw new ArgumentNullException(nameof(orgService));
            this.EntityName = typeof(TEntity).Name.ToLowerInvariant();
        }

        public TEntity Retrieve(Guid id, ColumnSet columns)
        {
            return OrgService.Retrieve(EntityName, id, columns).ToEntity<TEntity>();
        }

        public TEntity[] RetrieveMultiple(QueryExpression query)
        {
            return GetAllWithPaging(query, (Entity e) => e.ToEntity<TEntity>());
        }

        public TResult[] RetrieveMultiple<TResult>(QueryExpression query, Func<Entity, TResult> transformer)
        {
            return GetAllWithPaging(query, transformer);
        }

        private T[] GetAllWithPaging<T>(QueryExpression query, Func<Entity, T> transformer)
        {
            int? topCount = query.TopCount;
            query.TopCount = null;

            List<T> result = new List<T>();

            int pageNr = 1;

            query.PageInfo = new PagingInfo
            {
                PageNumber = pageNr,
                Count = topCount != null ? topCount.Value : 5000
            };

            while (true)
            {
                EntityCollection ecoll = OrgService.RetrieveMultiple(query);

                foreach (Entity entity in ecoll.Entities)
                {
                    var record = transformer(entity);
                    if (record != null)
                    {
                        result.Add(record);
                    }
                }

                if (ecoll.MoreRecords && topCount == null)
                {
                    pageNr++;
                    query.PageInfo.PageNumber = pageNr;
                    query.PageInfo.PagingCookie = ecoll.PagingCookie;
                }
                else
                {
                    break;
                }
            }

            return result.ToArray();
        }
    }
}
