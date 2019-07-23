using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace Xrm.Models.Interfaces
{
    public interface IQuery<TEntity> where TEntity : Entity
    {
        TEntity Retrieve(Guid id, ColumnSet columns);

        TEntity[] RetrieveMultiple(QueryExpression query);
    }
}
