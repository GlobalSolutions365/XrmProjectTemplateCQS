using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace DG.XrmContext
{
    internal class OptionSetValueCollection : List<OptionSetValue>
    {
        public OptionSetValueCollection(IEnumerable<OptionSetValue> collection) : base(collection)
        {
        }
    }
}