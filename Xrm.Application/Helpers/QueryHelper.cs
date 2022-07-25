using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace Xrm.Application.Helpers
{
    public static class QueryHelper
    {
        public static string GetParameterFromQuery(IOrganizationService orgService, IPluginExecutionContext pluginExecutionContext, string attributeName)
        {
            try
            {
                EntityCollection results = (EntityCollection)pluginExecutionContext.OutputParameters["BusinessEntityCollection"];

                FetchExpression fetch = (FetchExpression)pluginExecutionContext.InputParameters["Query"];
                FetchXmlToQueryExpressionResponse converted = (FetchXmlToQueryExpressionResponse)orgService.Execute(new FetchXmlToQueryExpressionRequest { FetchXml = fetch.Query });

                var condition = converted.Query.Criteria.Conditions.ToArray().Where(c => c.AttributeName == attributeName).FirstOrDefault();

                return condition.Values[0].ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"Error getting parameter \"{attributeName}\" from query.", ex);
            }
        }
    }
}
