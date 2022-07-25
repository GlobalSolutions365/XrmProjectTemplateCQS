using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Events
{
    public class TestTransactionalEvent2 : IEvent
    {
        public Account TargetAccount { get; set; }
    }
}
