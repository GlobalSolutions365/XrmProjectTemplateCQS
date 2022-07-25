using System;
using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Events
{
    public class TestTransactionalEvent1 : IEvent
    {
        public Guid ContactFromCommandId { get; set; }

        public Account TargetAccount { get; set; }
    }
}
