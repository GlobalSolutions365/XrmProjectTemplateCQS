using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Events
{
    public class TestTransactionalEvent2 : IEvent
    {
        public Account TargetAccount { get; set; }
    }
}
